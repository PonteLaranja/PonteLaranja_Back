using API.Domains;

namespace API.Interfaces
{
    public interface IItemRepository
    {
        public List<Item> Listar();
        public Item ObterPorId(Guid id);
        public List<Item> ListarPorTipo(Guid idTipo);
        public void Adicionar(Item item);
        public void Deletar(Guid id);
        public void Atualizar( Item item);

    }
}
