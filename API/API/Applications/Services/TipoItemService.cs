using API.Applications.Conversoes;
using API.DTOs.TipoItemDto;
using API.Interfaces;

namespace API.Applications.Services
{
    public class TipoItemService
    {
        private readonly ITipoItemRepository _repository;
        public TipoItemService(ITipoItemRepository repository)
        {
            _repository = repository;
        }

        public List<LerTipoItemDto> Listar()
        {
            return _repository.Listar().Select(l => TipoItemConversao.ConverterParaDto(l)).ToList();
        }

        public LerTipoItemDto ObterPorNome(string nome)
        {
            return TipoItemConversao.ConverterParaDto(_repository.ObterPorNome(nome));
        }
    }
}
