using API.Applications.Conversoes;
using API.DTOs.EstadoTransferenciaDto;
using API.Interfaces;

namespace API.Applications.Services
{
    public class EstadoTransferenciaService
    {
        private readonly IEstadoTransferenciaRepository _estadoTransferenciaRepository;
        public EstadoTransferenciaService(IEstadoTransferenciaRepository estadoTransferenciaRepository)
        {
            _estadoTransferenciaRepository = estadoTransferenciaRepository;
        }
        public List<LerEstadoTransferenciaDto> Listar()
        {
            return _estadoTransferenciaRepository.Listar().Select(l => EstadoTransferenciaConversao.ConverterParaDto(l)).ToList();
        }
        public LerEstadoTransferenciaDto ObterPorNome(string nome)
        {
            return EstadoTransferenciaConversao.ConverterParaDto(_estadoTransferenciaRepository.ObterPorNome(nome));
        }
         public bool EstadoTransferenciaExiste(Guid id)
        {
            return _estadoTransferenciaRepository.EstadoTransferenciaExiste(id);
        }
    }
}
