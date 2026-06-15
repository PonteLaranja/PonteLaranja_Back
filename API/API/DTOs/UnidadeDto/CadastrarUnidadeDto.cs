namespace API.DTOs.UnidadeDto
{
    public class CadastrarUnidadeDto
    {
        public string NomeUnidadeDto { get; set; } = null!;

        public string CEPDto { get; set; } = null!;

        public Guid UsuarioResponsavelDto { get; set; }

        public Guid TipoUnidadeDto { get; set; }
    }
}
