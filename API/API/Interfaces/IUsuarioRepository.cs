using API.Domains;

namespace API.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
        Usuario ObterPorId(Guid id);
        Usuario ObterPorEmail(string email);
        public bool EmailExiste(string email);
        bool UsuarioExiste(Guid id);
        void Cadastrar(Usuario usuario);
        void Atualizar(Guid id, Usuario usuario);
        void Deletar(Guid id);
    }
}