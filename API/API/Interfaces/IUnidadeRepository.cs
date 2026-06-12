using API.Domains;

namespace API.Interfaces
{
    public interface IUnidadeRepository
    {
        List<CadastrarOuAtualizarUnidadeDto> Listar();

        CadastrarOuAtualizarUnidadeDto BuscarPorGuid(Guid id);

        CadastrarOuAtualizarUnidadeDto BuscarPorNome(string nomeUnidade);

        CadastrarOuAtualizarUnidadeDto BuscarPorCEP(string cep);

        Usuario BuscarUsuario(Guid id);

        TipoUnidade BuscarTipoUnidade(Guid id);

        void Cadastrar(CadastrarOuAtualizarUnidadeDto unidadeNova, Guid responsavelId, Guid tipoUnidadeId);

        void Atualizar(CadastrarOuAtualizarUnidadeDto unidadeAtualizada, Guid responsavelId, Guid tipoUnidadeId);

        void Deletar(Guid unidadeId);
    }
}