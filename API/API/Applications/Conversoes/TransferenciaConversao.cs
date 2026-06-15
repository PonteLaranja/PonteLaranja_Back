using API.Domains;
using API.DTOs.TransferenciaDto;

namespace API.Applications.Conversoes
{
    public class TransferenciaConversao
    {
        public static LerTransferenciaDto ConverterParaDto(Transferencia dto)
        {
            return new LerTransferenciaDto
            {
                TransferenciaID = dto.TransferenciaID,
                ItemID = dto.ItemID,
                ItemCategoria = dto.Item.TipoItem.NomeTipoItem,
                DataChegada = dto.DataChegada,
                DataEnvio = dto.DataEnvio,
                EstadoTransferenciaID = dto.EstadoTransferenciaID,
                UnidadeDestinoID = dto.UnidadeDestinoID,
                UnidadeDestinoNome = dto.UnidadeDestino.NomeUnidade,
                UnidadeOrigemID = dto.UnidadeOrigemID,
                UnidadeOrigemNome = dto.UnidadeOrigem.NomeUnidade
            };
        }

        public static Transferencia ConverterParaDomain(CriarTransferenciaDto dto)
        {
            return new Transferencia
            {
                ItemID = dto.ItemID,
                DataChegada = dto.DataChegada,
                DataEnvio = dto.DataEnvio,
                EstadoTransferenciaID = dto.EstadoTransferenciaID,
                UnidadeDestinoID = dto.UnidadeDestinoID,
                UnidadeOrigemID = dto.UnidadeOrigemID,
            };
        }
    }
}
