using API.Domains;
using API.DTOs.TipoUsuarioDto;

namespace API.Applications.Conversoes
{
    public class TipoUsuarioConversao
    {
        public static LerTipoUsuarioDto ConverterParaDto(TipoUsuario tipo)
        {
            return new LerTipoUsuarioDto
            {
                Id = tipo.TipoUsuarioID,
                NomeTipoUsuario = tipo.NomeTipoUsuario
            };
        }
    }
}
