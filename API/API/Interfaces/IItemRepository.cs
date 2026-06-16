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
        public bool UnidadeExiste(Guid id);
        public bool TipoMedidaExiste(Guid id);
        public bool UsuarioExiste(Guid id);
        public bool TipoItemExiste(Guid id);
        public List<Item> ListarPorDoador(Guid usuarioId);
    }
}
