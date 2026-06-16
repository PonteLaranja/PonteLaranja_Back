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

        // Retorna a quantidade total de itens ativos filtrados por tipo e, opcionalmente, por tamanho (ex: Roupas).
        int ObterEstoqueAtualPorTipoItem(Guid unidadeId, Guid tipoItemId, string? tamanho = null);

        // Retorna o total de baixas sazonais filtradas por tipo, mês e, opcionalmente, por tamanho.
        int ObterTotalBaixasSazonais(Guid unidadeId, Guid tipoItemId, int mes, string? tamanho = null);
    }
}