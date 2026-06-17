using API.Contexts;
using API.Domains;
using API.Interfaces;


namespace API.Repositories
{
    public class TipoUnidadeRepository : ITipoUnidadeRepository
    {

        private readonly PonteLaranjaDbContext _context;

        public TipoUnidadeRepository(PonteLaranjaDbContext context)
        {
            _context = context;
        }

        public List<TipoUnidade> Listar()
        {
            return _context.TipoUnidade.ToList();
        }

        public TipoUnidade ObterPorNome(string nome)
        {
            return _context.TipoUnidade.FirstOrDefault(t => t.NomeTipoUnidade == nome);
        }
    }
}
