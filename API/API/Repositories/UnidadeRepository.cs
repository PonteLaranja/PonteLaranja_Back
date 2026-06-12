using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace API.Repositories
{
    public class UnidadeRepository : IUnidadeRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public UnidadeRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<CadastrarOuAtualizarUnidadeDto> Listar()
        {
            return _context.Unidade
                .OrderBy(Unidade => Unidade.NomeUnidade)
                .AsNoTracking()
                .ToList();
        }

        public CadastrarOuAtualizarUnidadeDto BuscarPorGuid(Guid id)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.UnidadeID == id);
        }

        public CadastrarOuAtualizarUnidadeDto BuscarPorCEP(string cep)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.CEP == cep);
        }

        public CadastrarOuAtualizarUnidadeDto BuscarPorNome(string nomeUnidade)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.NomeUnidade.ToLower() == nomeUnidade.ToLower());
        }

        public Usuario BuscarUsuario(Guid id)
        {
            return _context.Usuario
                .AsNoTracking()
                .FirstOrDefault(Usuario => Usuario.UsuarioID == id);
        }

        public TipoUnidade BuscarTipoUnidade(Guid id)
        {
            return _context.TipoUnidade
                .AsNoTracking()
                .FirstOrDefault(TipoUnidade => TipoUnidade.TipoUnidadeID == id);
        }

        public void Cadastrar(CadastrarOuAtualizarUnidadeDto unidadeNova, Guid responsavelId, Guid tipoUnidadeId)
        {
            CadastrarOuAtualizarUnidadeDto unidade = BuscarPorGuid(unidadeNova.UnidadeID);
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

            _context.Unidade.Add(unidadeNova);
            _context.SaveChanges();
        }

        public void Atualizar(CadastrarOuAtualizarUnidadeDto unidadeAtualizada, Guid responsavelId, Guid tipoUnidadeId)
        {
            CadastrarOuAtualizarUnidadeDto unidade = BuscarPorGuid(unidadeAtualizada.UnidadeID);
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

        public void Deletar(Guid unidadeId)
        {
            CadastrarOuAtualizarUnidadeDto unidade = BuscarPorGuid(unidadeId);
            if (unidade == null)
            {
                return;
            }

            _context.Unidade.Remove(unidade);
            _context.SaveChanges();
        }
    }
}