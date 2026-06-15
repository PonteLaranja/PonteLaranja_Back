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
            return _context.Transferencia.AsNoTracking()
                .Include(t => t.EstadoTransferencia)
                .Include(t => t.Item)
                .Include(t => t.Item.TipoItem)
                .Include(t => t.UnidadeDestino)
                .Include(t => t.UnidadeOrigem)
                .ToList();
        }

        public Transferencia ObterPorId(Guid id)
        {
            return _context.Transferencia
                .Include(t => t.EstadoTransferencia)
                .Include(t => t.Item)
                .Include(t => t.Item.TipoItem)
                .Include(t => t.UnidadeDestino)
                .Include(t => t.UnidadeOrigem)
                .FirstOrDefault(t => t.TransferenciaID == id);
        }

        public List<Transferencia> ObterPorUnidadeDestino(Guid id)
        {
            return _context.Transferencia
                .Include(t => t.EstadoTransferencia)
                .Include(t => t.Item)
                .Include(t => t.Item.TipoItem)
                .Include(t => t.UnidadeDestino)
                .Include(t => t.UnidadeOrigem)
                .Where(t => t.UnidadeDestinoID == id).ToList();
        }

        public List<Transferencia> ObterPorUnidadeOrigem(Guid id)
        {
            return _context.Transferencia
                .Include(t => t.EstadoTransferencia)
                .Include(t => t.Item)
                .Include(t => t.Item.TipoItem)
                .Include(t => t.UnidadeDestino)
                .Include(t => t.UnidadeOrigem)
                .Where(t => t.UnidadeOrigemID == id).ToList();
        }

        public bool ItemExiste(Guid id)
        {
            return _context.Item.Any(i => i.ItemID == id);
        }

        public bool UnidadeExiste(Guid id)
        {
            return _context.Unidade.Any(u => u.UnidadeID == id);
        }

        public void Cadastrar(Transferencia transferencia)
        {
            _context.Add(transferencia);
            _context.SaveChanges();
        }

        public void Atualizar(Guid id, Transferencia transferencia)
        {
            Transferencia transferenciaExistente = _context.Transferencia.Find(id);
            if (transferenciaExistente == null)
            {
                return;
            }
            transferenciaExistente.UnidadeOrigemID = transferencia.UnidadeOrigemID;
            transferenciaExistente.UnidadeDestinoID = transferencia.UnidadeDestinoID;
            transferenciaExistente.DataEnvio = transferencia.DataEnvio;
            transferenciaExistente.ItemID = transferencia.ItemID;

            _context.SaveChanges();
        }
    }
}
