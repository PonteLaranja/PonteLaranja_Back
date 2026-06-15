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
                .OrderBy(Unidade => Unidade.NomeUnidade)
                .AsNoTracking()
                .ToList();
        }

        public Unidade BuscarPorGuid(Guid id)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.UnidadeID == id);
        }

        public Unidade BuscarPorGuidComChangeTracker(Guid id)
        {
            return _context.Unidade
                .FirstOrDefault(Unidade => Unidade.UnidadeID == id);
        }

        public Unidade BuscarPorCEP(string cep)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.CEP == cep);
        }

        public Unidade BuscarPorNome(string nomeUnidade)
        {
            return _context.Unidade
                .AsNoTracking()
                .FirstOrDefault(Unidade => Unidade.NomeUnidade.ToLower() == nomeUnidade.ToLower());
        }

        public Usuario BuscarUsuario(Guid id)
        {
            return _context.Usuario
                .FirstOrDefault(Usuario => Usuario.UsuarioID == id);
        }

        public TipoUnidade BuscarTipoUnidade(Guid id)
        {
            return _context.TipoUnidade
                .FirstOrDefault(TipoUnidade => TipoUnidade.TipoUnidadeID == id);
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
            Unidade unidade = BuscarPorGuid(unidadeId);
            if (unidade == null)
            {
                return;
            }

            _context.Remove(unidade);
            _context.SaveChanges();
        }
    }
}