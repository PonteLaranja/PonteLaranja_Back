using API.Domains;
using API.DTOs.TipoUnidadeDto;

namespace API.Applications.Conversoes
{
    public class TipoUnidadeConversao
    {
        public static LerTipoUnidadeDto ConverterParaDto(TipoUnidade tipo)
        {
            return new LerTipoUnidadeDto
            {
                TipoUnidadeID = tipo.TipoUnidadeID,
                TipoUnidadeNome = tipo.NomeTipoUnidade
            };
        }
    }
}
