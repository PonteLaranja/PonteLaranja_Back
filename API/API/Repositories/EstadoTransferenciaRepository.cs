using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EstadoTransferenciaRepository : IEstadoTransferenciaRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public EstadoTransferenciaRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }
         public List<EstadoTransferencia> Listar()
        {
            return _context.EstadoTransferencia.AsNoTracking().ToList();
        }
        public EstadoTransferencia ObterPorNome(string nome)
        {
            return _context.EstadoTransferencia.AsNoTracking().FirstOrDefault(et => et.EstadoTransNome == nome);
        }
         public bool EstadoTransferenciaExiste(Guid id)
        {
            return _context.EstadoTransferencia.Any(et => et.EstadoTransID == id);
        }
    }
}
