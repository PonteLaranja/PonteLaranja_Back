using API.Domains;
using API.DTOs.TipoItemDto;

namespace API.Applications.Conversoes
{
    public class TipoItemConversao
    {
        public static LerTipoItemDto ConverterParaDto(TipoItem item)
        {
            return new LerTipoItemDto
            {
                TipoItemID = item.TipoItemID,
                NomeTipoItem = item.NomeTipoItem,
            };
        }
    }
}
