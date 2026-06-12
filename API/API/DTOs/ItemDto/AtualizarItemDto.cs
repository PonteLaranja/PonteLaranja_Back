namespace API.DTOs.ItemDto
{
    public class AtualizarItemDto
    {
        public Guid ItemID { get; set; }
        public string NomeItem { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public DateTime? DataDoacao { get; set; }
        public decimal Medida { get; set; }
        public int Quantidade { get; set; }
        public Guid? TipoItemID { get; set; }
        public Guid? TipoMedidaID { get; set; }
        public Guid? UsuarioID { get; set; }
    }
}
