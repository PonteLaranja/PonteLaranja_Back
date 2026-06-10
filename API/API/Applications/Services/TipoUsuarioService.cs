using API.Applications.Conversoes;
using API.DTOs.TipoUsuarioDto;
using API.Interfaces;

namespace API.Applications.Services
{
    public class TipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _repository;

        public TipoUsuarioService(ITipoUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<LerTipoUsuarioDto> Listar()
        {
            return _repository.Listar().Select(t => TipoUsuarioParaDto.ConverterParaDto(t)).ToList();
        }

        public LerTipoUsuarioDto ObterPorId(Guid id)
        {
            var tipo = _repository.ObterPorId(id);
            if (tipo == null)
            {
                return null;
            }
            return TipoUsuarioParaDto.ConverterParaDto(tipo);
        }
        public LerTipoUsuarioDto ObterPorNome(string nome)
        {
            var tipo = _repository.ObterPorNome(nome);
            if (tipo == null)
            {
                return null;
            }
            return TipoUsuarioParaDto.ConverterParaDto(tipo);
        }
    }
}
