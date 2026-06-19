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

        public List<Transferencia> ObterTransferenciasParaLog(Guid unidadeId, Guid? tipoItemId, DateTime? dataFiltro)
        {
            // Query básica trazendo os relacionamentos e aplicando AsNoTracking para leitura rápida
            var query = _context.Transferencia.AsNoTracking()
                .Include(t => t.EstadoTransferencia)
                .Include(t => t.Item)
                    .ThenInclude(i => i.Usuario)    // Essencial para pegar o nome do Doador na Service
                .Include(t => t.Item)
                    .ThenInclude(i => i.TipoMedida) // Essencial para pegar a Medida/TipoMedida
                .Include(t => t.Item.TipoItem)
                .Include(t => t.UnidadeDestino)
                .Include(t => t.UnidadeOrigem)
                // Filtra registros onde a unidade consultada participa como ORIGEM OU DESTINO
                // No banco deve haver uma unidade fictícia para simular doações às unidades de coleta, para não tornar o atributo unidade de origem como nulável
                .Where(t => t.UnidadeDestinoID == unidadeId || t.UnidadeOrigemID == unidadeId);

            // Filtro opcional por tipo de item (Alimento, Roupa, etc.)
            if (tipoItemId.HasValue)
            {
                query = query.Where(t => t.Item.TipoItemID == tipoItemId.Value);
            }

            // Filtro opcional por Data (compara apenas o dia, ignorando horas/minutos)
            if (dataFiltro.HasValue)
            {
                query = query.Where(t => t.DataEnvio.HasValue && t.DataEnvio.Value.Date == dataFiltro.Value.Date);
            }

            // Ordena por data mais recente primeiro e executa
            return query.OrderByDescending(t => t.DataEnvio).ToList();
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
            transferenciaExistente.EstadoTransferenciaID = transferencia.EstadoTransferenciaID;

            _context.SaveChanges();
        }
    }
}
