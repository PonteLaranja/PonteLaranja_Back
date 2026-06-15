using API.Domains;
using API.DTOs.ItemDto;

namespace API.Applications.Conversoes
{
    public class ItemConversao
    {
        public static LerItemDto ConverterParaDto(Item item)
        {
            return new LerItemDto
            {
                ItemID = item.ItemID,
                NomeItem = item.NomeItem,
                DataRecebimento = item.DataRecebimento,
                DataDoacao = item.DataDoacao,
                Medida = item.Medida,
                Quantidade = item.Quantidade,
                TipoItemID = item.TipoItemID,
                TipoMedidaID = item.TipoMedidaID,
                UsuarioID = item.UsuarioID
            };
        }

        public static Item ConverterParaDominioAdcionar(AdicionarItemDto criarItemDto)
        {
                return new Item
                {
                    NomeItem = criarItemDto.NomeItem,
                    DataRecebimento = criarItemDto.DataRecebimento,
                    DataDoacao = criarItemDto.DataDoacao,
                    Medida = criarItemDto.Medida,
                    Quantidade = criarItemDto.Quantidade,
                    TipoItemID = criarItemDto.TipoItemID,
                    TipoMedidaID = criarItemDto.TipoMedidaID,
                    UsuarioID = criarItemDto.UsuarioID
                };
        }
        public static Item ConverterParaDominioAtualizar(AtualizarItemDto atualizarItemDto)
        {
            return new Item
            {
                NomeItem = atualizarItemDto.NomeItem,
                DataRecebimento = atualizarItemDto.DataRecebimento,
                DataDoacao = atualizarItemDto.DataDoacao,
                Medida = atualizarItemDto.Medida,
                Quantidade = atualizarItemDto.Quantidade,
                TipoItemID = atualizarItemDto.TipoItemID,
                TipoMedidaID = atualizarItemDto.TipoMedidaID,
                UsuarioID = atualizarItemDto.UsuarioID
            };
        }
    }
}
