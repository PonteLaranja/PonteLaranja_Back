namespace API.DTOs.ItemDto
{
    public class AdicionarItemDto
    {
        public string NomeItem { get; set; }
        public DateTime DataRecebimento { get; set; }
        public DateTime? DataDoacao { get; set; }
        public decimal Medida { get; set; }
        public int Quantidade { get; set; }
        public Guid TipoItemID { get; set; }
        public Guid TipoMedidaID { get; set; }
        public Guid UsuarioID { get; set; }
        public Guid UnidadeID { get; set; }
    }
}
