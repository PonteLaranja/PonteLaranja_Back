using API.Domains;

namespace API.Interfaces
{
    public interface ITipoUsuarioRepository
    {
        List<TipoUsuario> Listar();
        TipoUsuario ObterPorId(Guid id);
        TipoUsuario ObterPorNome(string nome);
    }
}