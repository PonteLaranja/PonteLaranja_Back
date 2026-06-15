using API.Domains;
using API.DTOs.TipoMedidaDto;

namespace API.Applications.Conversoes
{
    public class TipoMedidaConversao
    {
        public static LerTipoMedidaDto ConverterParaDto(TipoMedida medida)
        {
            return new LerTipoMedidaDto
            {
                TipoMedidaID = medida.TipoMedidaID,
                NomeTipoMedida = medida.NomeTipoMedida
            };
        }
    }
}
