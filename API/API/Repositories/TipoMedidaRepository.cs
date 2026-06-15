using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TipoMedidaRepository : ITipoMedidaRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public TipoMedidaRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<TipoMedida> Listar()
        {
            return _context.TipoMedida.AsNoTracking().ToList();
        }

        public TipoMedida ObterPorNome(string nome)
        {
            return _context.TipoMedida.FirstOrDefault(tm => tm.NomeTipoMedida == nome);
        }

        public bool TipoMedidaExiste(Guid id)
        {
            return _context.TipoMedida.Any(tm => tm.TipoMedidaID == id);
        }
    }
}
