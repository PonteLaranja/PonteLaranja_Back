using API.Applications.Conversoes;
using API.DTOs.ItemDto;
using API.Interfaces;
using API.Exceptions;

namespace API.Applications.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repository;
        public ItemService(IItemRepository repository) { 
            _repository = repository;
        }

        private bool ValidarIds(Guid? idTipoItem, Guid? medidaId, Guid? usuarioId, Guid? unidadeId)
        {
            if (idTipoItem == null && !_repository.TipoItemExiste(idTipoItem.Value))
            {
                throw new DomainException("Tipo de item não encontrado.");
            }

            if(medidaId == null && !_repository.TipoMedidaExiste(medidaId.Value))
            {
                throw new DomainException("Tipo de medida não encontrado.");
            }

            if (usuarioId == null && !_repository.UsuarioExiste(usuarioId.Value))
            {
                throw new DomainException("Usuário não encontrado.");
            }

            if (unidadeId == null && !_repository.UnidadeExiste(unidadeId.Value))
            {
                throw new DomainException("Unidade não encontrada.");
            }
            return true;
        }
        public List<LerItemDto> Listar()
        {
            return _repository.Listar().Select(i => ItemConversao.ConverterParaDto(i)).ToList();
        }

        public List<LerItemDto> ListarPorTipo(Guid idTipo)
        {
            List<LerItemDto> itens = _repository.ListarPorTipo(idTipo).Select(i => ItemConversao.ConverterParaDto(i)).ToList();

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

            _repository.Adicionar(ItemConversao.ConverterParaDominioAdcionar(criarItemDto));
        }

        public void Atualizar(Guid id, AtualizarItemDto atualizarItemDto)
        {
            var itemBanco = _repository.ObterPorId(id);

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

            _repository.Atualizar(itemBanco);
        }

        public void Inativar(Guid id)
        {
            var itemBanco = _repository.ObterPorId(id);
            if (itemBanco == null)
            {
                throw new DomainException("O item a ser deletado não existe.");
            }
            _repository.Deletar(id);
        }
    }
}
