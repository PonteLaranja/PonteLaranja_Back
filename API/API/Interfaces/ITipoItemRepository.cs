using API.Domains;

namespace API.Interfaces
{
    public interface ITipoItemRepository
    {
        public List<TipoItem> Listar();
        public TipoItem ObterPorNome(string nome);
        bool TipoItemExiste(Guid id);
    }
}
