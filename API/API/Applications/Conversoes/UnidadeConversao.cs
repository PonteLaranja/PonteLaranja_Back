using API.Domains;
using API.DTOs.UnidadeDto;

namespace API.Applications.Conversoes
{
    public class UnidadeConversao
    {
        public static LerUnidadeDto LerDto(Unidade unidade)
        {
            return new LerUnidadeDto
            {
                UnidadeDto = unidade.UnidadeID,
                NomeUnidadeDto = unidade.NomeUnidade,
                CEPDto = unidade.CEP,
                UsuarioResponsavelDto = unidade.UsuarioResponsavelID,
                TipoUnidadeDto = unidade.TipoUnidadeID
            };
        }

        public static Unidade CadastrarOuAtualizar(CadastrarUnidadeDto criarDto, Guid? id = null)
        {
            return new Unidade
            {
                UnidadeID = id ?? Guid.Empty,
                NomeUnidade = criarDto.NomeUnidadeDto,
                CEP = criarDto.CEPDto,
                UsuarioResponsavelID = criarDto.UsuarioResponsavelDto,
                TipoUnidadeID = criarDto.TipoUnidadeDto
            };
        }
    }
}