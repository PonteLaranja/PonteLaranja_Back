using API.Applications.Conversoes;
using API.DTOs.ItemDto;
using API.Interfaces;
using API.Exceptions;

namespace API.Applications.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ITipoItemRepository _tipoItemRepository;
        private readonly ITipoMedidaRepository _tipoMedidaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnidadeRepository _unidadeRepository;
        public ItemService(IItemRepository itemRepository, ITipoItemRepository tipoItemRepository, ITipoMedidaRepository tipoMedidaRepository, IUsuarioRepository usuarioRepository, IUnidadeRepository unidadeRepository)
        {
            _itemRepository = itemRepository;
            _tipoItemRepository = tipoItemRepository;
            _tipoMedidaRepository = tipoMedidaRepository;
            _usuarioRepository = usuarioRepository;
            _unidadeRepository = unidadeRepository;
        }

        private bool ValidarIds(Guid? idTipoItem, Guid? medidaId, Guid? usuarioId, Guid? unidadeId)
        {
            if (idTipoItem == null && !_tipoItemRepository.TipoItemExiste(idTipoItem.Value))
            {
                throw new DomainException("Tipo de item não encontrado.");
            }

            if(medidaId == null && !_tipoMedidaRepository.TipoMedidaExiste(medidaId.Value))
            {
                throw new DomainException("Tipo de medida não encontrado.");
            }

            if (usuarioId == null && !_usuarioRepository.UsuarioExiste(usuarioId.Value))
            {
                throw new DomainException("Usuário não encontrado.");
            }

            //if (unidadeId == null && !_unidadeRepository.UnidadeExiste(unidadeId.Value))
            //{
            //    throw new DomainException("Unidade não encontrada.");
            //}
            return true;
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

            ValidarIds(criarItemDto.TipoItemID, criarItemDto.TipoMedidaID, criarItemDto.UsuarioID, criarItemDto.UnidadeID);

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

            ValidarIds(atualizarItemDto.TipoItemID, atualizarItemDto.TipoMedidaID, atualizarItemDto.UsuarioID, atualizarItemDto.UnidadeID);

            if (atualizarItemDto.NomeItem != null)
            itemBanco.NomeItem = atualizarItemDto.NomeItem;

            if (atualizarItemDto.DataRecebimento != null)
                itemBanco.DataRecebimento = atualizarItemDto.DataRecebimento;

            if (atualizarItemDto.DataDoacao != null)
                itemBanco.DataDoacao = atualizarItemDto.DataDoacao;

            if (atualizarItemDto.Medida != 0)
                itemBanco.Medida = atualizarItemDto.Medida;

            if (atualizarItemDto.Quantidade != 0)
                itemBanco.Quantidade = atualizarItemDto.Quantidade;

            if (atualizarItemDto.TipoItemID != null)
                itemBanco.TipoItemID = atualizarItemDto.TipoItemID;

            if (atualizarItemDto.TipoMedidaID != null)
                itemBanco.TipoMedidaID = atualizarItemDto.TipoMedidaID;

            if (atualizarItemDto.UsuarioID != null)
                itemBanco.UsuarioID = atualizarItemDto.UsuarioID;

            if (atualizarItemDto.UnidadeID != null)
                itemBanco.UnidadeID = atualizarItemDto.UnidadeID;
           
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
