using API.Domains;

namespace API.DTOs.TransferenciaDto
{
    public class LerTransferenciaDto
    {
        public Guid TransferenciaID { get; set; }

        public DateTime? DataEnvio { get; set; }

        public DateTime? DataChegada { get; set; }

        public Guid UnidadeOrigemID { get; set; }
        public string UnidadeOrigemNome { get; set; }

        public Guid UnidadeDestinoID { get; set; }
        public string UnidadeDestinoNome { get; set; }

        public Guid EstadoTransferenciaID { get; set; }

        public Guid ItemID { get; set; }
        public string ItemNome { get; set; }
        public string ItemCategoria { get; set; }
    }
}
