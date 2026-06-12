namespace API.DTOs.UsuarioDto
{
    public class LerUsuarioDto
    {
        public Guid UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DataCriação { get; set; }
        public Guid TipoUsuarioID { get; set; }       
    }
}
