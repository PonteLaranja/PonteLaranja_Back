using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PonteLaranjaDbContext _context;
        public UsuarioRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario.AsNoTracking().ToList();
        }

        public Usuario ObterPorId(Guid id)
        {
            return _context.Usuario
                .Include(u => u.TipoUsuario)
                .FirstOrDefault(u => u.UsuarioID == id);
        }

        public bool UsuarioExiste(Guid id)
        {
            return _context.Usuario.Any(u => u.UsuarioID == id);
        }

        public Usuario ObterPorEmail(string email)
        {
            return _context.Usuario.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailExiste(string email)
        {
            return _context.Usuario.Any(u => u.Email == email);
        }

        public void Cadastrar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Guid id, Usuario usuario)
        {
            Usuario usuarioExistente = _context.Usuario.Find(id);
            if (usuarioExistente == null)
            {
                return;
            }

            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Senha = usuario.Senha;
            usuarioExistente.TipoUsuarioID = usuario.TipoUsuarioID;
            _context.SaveChanges();
        }

        public void Deletar(Guid id)
        {
            Usuario usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return;
            }
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
