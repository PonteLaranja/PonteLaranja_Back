using API.Applications.Conversoes;
using API.DTOs.ItemDto;
using API.Interfaces;
using API.Exceptions;

namespace API.Applications.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public List<LerItemDto> Listar()
        {
            return _itemRepository.Listar().Select(i => ItemConversao.ConverterParaDto(i)).ToList();
        }

        public List<LerItemDto> ListarPorTipo(Guid idTipo)
        {
            List<LerItemDto> itens = _itemRepository.ListarPorTipo(idTipo).Select(i => ItemConversao.ConverterParaDto(i)).ToList();

            if (itens == null || itens.Count == 0)
            {
                throw new DomainException("Nenhum item encontrado para o tipo especificado.");
            }

            return itens;
        }

        public void Adicionar(AdicionarItemDto criarItemDto)
        {
            if (criarItemDto == null)
            {
                throw new DomainException("O item a ser adicionado não pode ser nulo.");
            }

            _itemRepository.Adicionar(ItemConversao.ConverterParaDominioAdcionar(criarItemDto));
        }

        public void Atualizar(Guid id, AtualizarItemDto atualizarItemDto)
        {
            var itemBanco = _itemRepository.ObterPorId(id);

            if (itemBanco == null)
            {
                throw new DomainException("O item a ser atualizado não existe.");
            }

            if (atualizarItemDto == null)
            {
                throw new DomainException("O item a ser atualizado não pode ser nulo.");
            }

            itemBanco.NomeItem = atualizarItemDto.NomeItem;
            itemBanco.DataRecebimento = atualizarItemDto.DataRecebimento;
            itemBanco.DataDoacao = atualizarItemDto.DataDoacao;
            itemBanco.Medida = atualizarItemDto.Medida;
            itemBanco.Quantidade = atualizarItemDto.Quantidade;
            itemBanco.TipoItemID = atualizarItemDto.TipoItemID;
            itemBanco.TipoMedidaID = atualizarItemDto.TipoMedidaID;
            itemBanco.UsuarioID = atualizarItemDto.UsuarioID;
            itemBanco.UnidadeID = itemBanco.UnidadeID;
           
            _itemRepository.Atualizar(itemBanco);
        }

        public void Deletar(Guid id)
        {
            var itemBanco = _itemRepository.ObterPorId(id);
            if (itemBanco == null)
            {
                throw new DomainException("O item a ser deletado não existe.");
            }
            _itemRepository.Deletar(id);
        }
    }
}
