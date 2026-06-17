using API.Domains;

namespace API.DTOs.UnidadeDto
{
    public class LerUnidadeDto
    {
        public Guid UnidadeDto { get; set; }

        public string NomeUnidadeDto { get; set; } = string.Empty;

        public bool Ativo {  get; set; }

        public string CEPDto { get; set; } = string.Empty;

        public Guid UsuarioResponsavelDto { get; set; }
        public string NomeResponsavelDto { get; set; }

        public Guid TipoUnidadeDto { get; set; }
        public string NomeTipoUnidadeDto { get; set; }
    }
}
