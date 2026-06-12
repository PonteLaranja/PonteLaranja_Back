using API.Domains;

namespace API.Interfaces
{
    public interface IEstadoTransferenciaRepository
    {
        List<EstadoTransferencia> Listar();
        EstadoTransferencia ObterPorNome(string nome);
        bool EstadoTransferenciaExiste(Guid id);
    }
}
