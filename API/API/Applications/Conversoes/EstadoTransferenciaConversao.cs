using API.Domains;
using API.DTOs.EstadoTransferenciaDto;

namespace API.Applications.Conversoes
{
    public class EstadoTransferenciaConversao
    {
        public static LerEstadoTransferenciaDto ConverterParaDto(EstadoTransferencia estadoTransferencia)
        {
            return new LerEstadoTransferenciaDto
            {
                EstadoTransferenciaID = estadoTransferencia.EstadoTransID,
                NomeEstadoTransferencia = estadoTransferencia.EstadoTransNome
            };
        }
    }
}
