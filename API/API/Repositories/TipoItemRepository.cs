using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TipoItemRepository : ITipoItemRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public TipoItemRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }
        public List<TipoItem> Listar()
        {
            return _context.TipoItem.AsNoTracking().ToList();
        }

        public bool TipoItemExiste(Guid id)
        {
            return _context.TipoItem.Any(t => t.TipoItemID == id);
        }
        public TipoItem ObterPorNome(string nome)
        {
            return _context.TipoItem.AsNoTracking().FirstOrDefault(t => t.NomeTipoItem == nome);
        }
    }
}
