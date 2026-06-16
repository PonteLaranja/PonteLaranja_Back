using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PonteLaranjaDbContext _context;
        public ItemRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<Item> Listar()
        {
            return _context.Item
                .Include(i => i.TipoItem)
                .Include(i => i.Unidade)
                .Include(i => i.TipoMedida)
                .Include(i => i.Usuario)
                .AsNoTracking()
                .ToList();
        }

        public Item ObterPorId(Guid id)
        {
            return _context.Item
                .Include(i => i.TipoItem)
                .Include(i => i.Unidade)
                .Include(i => i.TipoMedida)
                .Include(i => i.Usuario)
                .AsNoTracking()
                .FirstOrDefault(item => item.ItemID == id);
        }

        public List<Item> ListarPorTipo(Guid idTipo)
        {
            return _context.Item
                .Where(item => item.TipoItemID == idTipo)
                .Include(i => i.TipoItem)
                .Include(i => i.Unidade)
                .Include(i => i.TipoMedida)
                .Include(i => i.Usuario)
                .AsNoTracking()
                .ToList();
        }
        public void Adicionar(Item item)
        {
            _context.Item.Add(item);
            _context.SaveChanges();
        }
        public void Deletar(Guid id)
        {
            _context.Item.Remove(_context.Item.Find(id));
            _context.SaveChanges();
        }

        public bool UnidadeExiste(Guid id)
        {
            return _context.Unidade.Any(u => u.UnidadeID == id);
        }

        public bool TipoItemExiste(Guid id)
        {
            return _context.TipoItem.Any(u => u.TipoItemID == id);
        }

        public bool UsuarioExiste(Guid id)
        {
            return _context.Usuario.Any(u => u.UsuarioID == id);
        }

        public bool TipoMedidaExiste(Guid id)
        {
            return _context.TipoMedida.Any(u => u.TipoMedidaID == id);
        }

        public void Atualizar(Item item)
        {
            if(item == null || !_context.Item.Any(i => i.ItemID == item.ItemID))
                return;

            Item itemBanco = _context.Item.Find(item.ItemID);
            itemBanco.NomeItem = item.NomeItem;
            itemBanco.DataRecebimento = item.DataRecebimento;
            itemBanco.DataDoacao = item.DataDoacao;
            itemBanco.Medida = item.Medida;
            itemBanco.Quantidade = item.Quantidade;
            itemBanco.TipoItemID = item.TipoItemID;
            itemBanco.TipoMedidaID = item.TipoMedidaID;
            itemBanco.UsuarioID = item.UsuarioID;
            itemBanco.UnidadeID = item.UnidadeID;

            _context.SaveChanges();
        }
    }
}
