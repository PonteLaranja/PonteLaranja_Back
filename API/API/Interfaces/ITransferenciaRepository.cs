using API.Domains;

namespace API.Interfaces
{
    public interface ITransferenciaRepository
    {
        List<Transferencia> Listar();

        Transferencia ObterPorId(Guid id);
        List<Transferencia> ObterPorUnidadeDestino(Guid id);
        List<Transferencia> ObterPorUnidadeOrigem(Guid id);

        List<Transferencia> ObterTransferenciasParaLog(Guid unidadeId, Guid? tipoItemId, DateTime? dataFiltro);

        void Cadastrar(Transferencia transferencia);
        void Atualizar(Guid id, Transferencia transferencia);

        bool ItemExiste(Guid id);
        bool UnidadeExiste(Guid id);
    }
}
