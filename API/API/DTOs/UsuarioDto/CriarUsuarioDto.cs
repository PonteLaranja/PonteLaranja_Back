namespace API.DTOs.UsuarioDto
{
    public class CriarUsuarioDto
    {
        public string UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string TipoUsuarioID { get; set; }
    }
}
