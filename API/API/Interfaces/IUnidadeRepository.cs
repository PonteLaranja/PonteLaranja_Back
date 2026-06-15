using API.Domains;

namespace API.Interfaces
{
    public interface IUnidadeRepository
    {
        List<Unidade> Listar();

        Unidade BuscarPorGuid(Guid id);

        Unidade BuscarPorNome(string nomeUnidade);

        Unidade BuscarPorCEP(string cep);

        Usuario BuscarUsuario(Guid id);

        TipoUnidade BuscarTipoUnidade(Guid id);

        void Cadastrar(Unidade unidadeNova, Guid responsavelId, Guid tipoUnidadeId);

        void Atualizar(Unidade unidadeAtualizada, Guid responsavelId, Guid tipoUnidadeId);

        void Inativar(Guid unidadeId);
    }
}