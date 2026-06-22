using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace API.Repositories
{
    public class UnidadeRepository : IUnidadeRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public UnidadeRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<Unidade> Listar()
        {
            return _context.Unidade
                .Where(u => u.Ativo)
                .OrderBy(Unidade => Unidade.NomeUnidade)
                .AsNoTracking()
                .Include(u => u.UsuarioResponsavel)
                .Include(u => u.TipoUnidade)
                .ToList();
        }

        public Unidade BuscarPorGuid(Guid id)
        {
            return _context.Unidade
                .Include(u => u.UsuarioResponsavel)
                .Include(u => u.TipoUnidade)
                .AsNoTracking()
                .FirstOrDefault(u => u.UnidadeID == id && u.Ativo);
        }

        public Unidade BuscarPorGuidComChangeTracker(Guid id)
        {
            return _context.Unidade
                .FirstOrDefault(u => u.UnidadeID == id && u.Ativo);
        }

        public Unidade BuscarPorCEP(string cep)
        {
            return _context.Unidade
                .Include(u => u.UsuarioResponsavel)
                .Include(u => u.TipoUnidade)
                .AsNoTracking()
                .FirstOrDefault(u => u.CEP == cep && u.Ativo);
        }

        public Unidade BuscarPorNome(string nomeUnidade)
        {
            return _context.Unidade
                .Include(u => u.UsuarioResponsavel)
                .Include(u => u.TipoUnidade)
                .AsNoTracking()
                .FirstOrDefault(u => u.NomeUnidade.ToLower() == nomeUnidade.ToLower() && u.Ativo);
        }

        public string? ObterNomeTipoItemPorId(Guid tipoItemId)
        {
            return _context.TipoItem
                .AsNoTracking()
                .Where(t => t.TipoItemID == tipoItemId)
                .Select(t => t.NomeTipoItem)
                .FirstOrDefault();
        }

        public Usuario BuscarUsuario(Guid id)
        {
            return _context.Usuario
                .FirstOrDefault(u => u.UsuarioID == id && u.Ativo);
        }

        public TipoUnidade BuscarTipoUnidade(Guid id)
        {
            return _context.TipoUnidade
                .FirstOrDefault(t => t.TipoUnidadeID == id);
        }

        public void Cadastrar(Unidade unidadeNova, Guid responsavelId, Guid tipoUnidadeId)
        {
            Unidade unidade = BuscarPorGuidComChangeTracker(unidadeNova.UnidadeID);
            if (unidade != null)
            {
                return;
            }

            Usuario usuario = BuscarUsuario(responsavelId);
            if (usuario == null)
            {
                return;
            }

            TipoUnidade tipoUnidade = BuscarTipoUnidade(tipoUnidadeId);
            if (tipoUnidade == null)
            {
                return;
            }

            Unidade unidadeCadastrada = new Unidade();

            unidadeCadastrada.NomeUnidade = unidadeNova.NomeUnidade;
            unidadeCadastrada.CEP = unidadeNova.CEP;
            unidadeCadastrada.UsuarioResponsavelID = responsavelId;
            unidadeCadastrada.TipoUnidadeID = tipoUnidadeId;

            _context.Add(unidadeCadastrada);
            _context.SaveChanges();
        }

        public void Atualizar(Unidade unidadeAtualizada, Guid responsavelId, Guid tipoUnidadeId)
        {
            Unidade unidade = BuscarPorGuidComChangeTracker(unidadeAtualizada.UnidadeID);
            if (unidade == null)
            {
                return;
            }

            Usuario usuario = BuscarUsuario(responsavelId);
            if (usuario == null)
            {
                return;
            }

            TipoUnidade tipoUnidade = BuscarTipoUnidade(tipoUnidadeId);
            if (tipoUnidade == null)
            {
                return;
            }

            unidade.NomeUnidade = unidadeAtualizada.NomeUnidade;
            unidade.CEP = unidadeAtualizada.CEP;
            unidade.UsuarioResponsavelID = responsavelId;
            unidade.TipoUnidadeID = tipoUnidadeId;

            _context.SaveChanges();
        }

        public void Inativar(Guid unidadeId)
        {
            Unidade unidade = BuscarPorGuidComChangeTracker(unidadeId);
            if (unidade == null)
            {
                return;
            }

            _context.Remove(unidade);
            _context.SaveChanges();
        }

        public int ObterEstoqueAtualPorTipoItem(Guid unidadeId, Guid tipoItemId, string? tamanho = null)
        {
            // 1. Busca os itens ativos na unidade e categoria alvo
            var query = _context.Item
                .AsNoTracking()
                .Include(i => i.TipoMedida)
                .Where(i => i.Ativo && i.UnidadeID == unidadeId && i.TipoItemID == tipoItemId);

            // 2. Filtra pelo tamanho (NomeTipoMedida) de forma performática se fornecido
            if (!string.IsNullOrEmpty(tamanho))
            {
                var tipoMedidaId = _context.TipoMedida
                    .AsNoTracking()
                    .Where(tm => tm.NomeTipoMedida == tamanho)
                    .Select(tm => tm.TipoMedidaID)
                    .FirstOrDefault();

                if (tipoMedidaId != Guid.Empty)
                {
                    query = query.Where(i => i.TipoMedidaID == tipoMedidaId);
                }
            }

            var itens = query.ToList();
            if (!itens.Any()) return 0;

            decimal totalCalculado = 0;

            foreach (var item in itens)
            {
                var tipoMedida = item.TipoMedida;

                // Multiplicação precisa defendida para Alimentos e Bebidas (Kg / Litro)
                if (tipoMedida != null && (tipoMedida.NomeTipoMedida == "Kg" || tipoMedida.NomeTipoMedida == "Litro"))
                {
                    totalCalculado += item.Medida * item.Quantidade;
                }
                else
                {
                    totalCalculado += item.Quantidade;
                }
            }

            return (int)Math.Ceiling(totalCalculado);
        }

        public int ObterTotalBaixasSazonais(Guid unidadeId, Guid tipoItemId, int mes, string? tamanho = null)
        {
            // Baixa real monitorada via Soft Delete (!Ativo) e DataDoacao preenchida na tabela Item
            var query = _context.Item
                .AsNoTracking()
                .Include(i => i.TipoMedida)
                .Where(i => !i.Ativo // Item doado/excluido do estoque final
                         && i.UnidadeID == unidadeId
                         && i.TipoItemID == tipoItemId
                         && i.DataDoacao != null // Verifica se a coluna DATETIME2 possui valor
                         && i.DataDoacao.Value.Month == mes); // Mapeia o mes da doação real

            if (!string.IsNullOrEmpty(tamanho))
            {
                var tipoMedidaId = _context.TipoMedida
                    .AsNoTracking()
                    .Where(tm => tm.NomeTipoMedida == tamanho)
                    .Select(tm => tm.TipoMedidaID)
                    .FirstOrDefault();

                if (tipoMedidaId != Guid.Empty)
                {
                    query = query.Where(i => i.TipoMedidaID == tipoMedidaId);
                }
            }

            var itensDoados = query.ToList();
            if (!itensDoados.Any()) return 0;

            decimal totalBaixas = 0;

            foreach (var item in itensDoados)
            {
                var tipoMedida = item.TipoMedida;

                if (tipoMedida != null && (tipoMedida.NomeTipoMedida == "Kg" || tipoMedida.NomeTipoMedida == "Litro"))
                {
                    totalBaixas += item.Medida * item.Quantidade;
                }
                else
                {
                    totalBaixas += item.Quantidade;
                }
            }

            return (int)Math.Ceiling(totalBaixas);
        }
    }
}