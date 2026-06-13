using API.Contexts;
using API.Domains;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly PonteLaranjaDbContext _context;

        public TransferenciaRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<Transferencia> Listar()
        {
            return _context.Transferencia.AsNoTracking().ToList();
        }

        public Transferencia ObterPorId(Guid id)
        {
            return _context.Transferencia.Find(id);
        }

        public List<Transferencia> ObterPorUnidadeDestino(Guid id)
        {
            return _context.Transferencia.Where(t => t.UnidadeDestinoID == id).ToList();
        }

        public List<Transferencia> ObterPorUnidadeOrigem(Guid id)
        {
            return _context.Transferencia.Where(t => t.UnidadeOrigemID == id).ToList();
        }

        public void Cadastrar(Transferencia transferencia)
        {
            _context.Add(transferencia);
            _context.SaveChanges();
        }

    }
}
