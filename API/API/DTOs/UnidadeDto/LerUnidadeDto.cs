using API.Domains;

namespace API.DTOs.UnidadeDto
{
    public class LerUnidadeDto
    {
        public Guid UnidadeDto { get; set; }

        public string NomeUnidadeDto { get; set; } = string.Empty;

        public string CEPDto { get; set; } = string.Empty;

        public Guid UsuarioResponsavelDto { get; set; }

        public Guid TipoUnidadeDto { get; set; }
    }
}
