namespace API.DTOs.TransferenciaDto
{
    public class LogDoacaoDto
    {
        public Guid LogItemId { get; set; }
        public Guid ItemId { get; set; }
        public string NomeItem { get; set; } = null!;
        public string UsuarioDoador { get; set; } = null!;
        public string Unidade { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal Medida { get; set; }
        public string TipoMedida { get; set; } = null!;
        public string? Tamanho { get; set; }          // Exclusivo para quando for Roupa
        public string FaseDoacao { get; set; } = null!; // "Recebimento" ou "Baixa"
        public DateTime Data { get; set; }
    }
}