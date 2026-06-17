using API.Domains;

namespace API.Interfaces
{
    public interface ITipoUnidadeRepository
    {
        public List<TipoUnidade> Listar();
        public TipoUnidade ObterPorNome(string nome);
    }
}
