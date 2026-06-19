namespace API.DTOs.TransferenciaDto
{
    public class ConsultarLogDto
    {
        public Guid UnidadeID { get; set; }
        public Guid? TipoItemID { get; set; }
        public string? FaseDoacao { get; set; } // "Recebimento" ou "Baixa"
        public DateTime? DataFiltro { get; set; }
    }
}