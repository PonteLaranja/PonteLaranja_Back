using API.Domains;

namespace API.Interfaces
{
    public interface ITipoMedidaRepository
    {
        List<TipoMedida> Listar();
        TipoMedida ObterPorNome(string nome);
        bool TipoMedidaExiste(Guid id);
    }
}
