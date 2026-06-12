using API.Applications.Conversoes;
using API.DTOs.TipoMedidaDto;
using API.Interfaces;

namespace API.Applications.Services
{
    public class TipoMedidaService
    {
        private readonly ITipoMedidaRepository _repository;

        public TipoMedidaService(ITipoMedidaRepository repository)
        {
            _repository = repository;
        }

        public List<LerTipoMedidaDto> Listar()
        {
            return _repository.Listar().Select(medida => TipoMedidaConversao.ConverterParaDto(medida)).ToList();
        }

        public LerTipoMedidaDto ObterPorNome(string nome)
        {
            var medida = _repository.ObterPorNome(nome);
            if (medida == null)
            {
                throw new Exception("Tipo de medida não encontrado");
            }
            return TipoMedidaConversao.ConverterParaDto(medida);
        }
    }
}
