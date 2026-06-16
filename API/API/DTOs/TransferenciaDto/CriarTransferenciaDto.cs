namespace API.DTOs.TransferenciaDto
{
    public class CriarTransferenciaDto
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataChegada { get; set; }
        public Guid UnidadeOrigemID { get; set; }
        public Guid UnidadeDestinoID { get; set; }
        public Guid EstadoTransferenciaID { get; set; } 
        public Guid ItemID { get; set; }
    }
}
