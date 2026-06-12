using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly PonteLaranjaDbContext _context;
        public TipoUsuarioRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }
        public List<TipoUsuario> Listar()
        {
            return _context.TipoUsuario.AsNoTracking().ToList();
        }

        public TipoUsuario ObterPorId(Guid id)
        {
            return _context.TipoUsuario.Find(id);
        }

        public TipoUsuario ObterPorNome(string nome)
        {
            return _context.TipoUsuario.FirstOrDefault(tu => tu.NomeTipoUsuario == nome);
        }

        public bool TipoUsuarioExiste(Guid id)
        {
            return _context.TipoUsuario.Any(tu => tu.TipoUsuarioID == id);
        }
    }
}
