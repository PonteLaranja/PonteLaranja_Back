using API.Domains;

namespace API.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
        Usuario ObterPorId(Guid id);
        Usuario ObterPorEmail(string email);
        void Cadastrar(Usuario usuario);
        void Atualizar(Guid id, Usuario usuario);
        void Deletar(string id);
    }
}