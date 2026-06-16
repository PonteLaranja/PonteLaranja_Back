using API.Applications.Conversoes;
using API.DTOs.TipoUnidadeDto;
using API.Interfaces;
using API.Exceptions;

namespace API.Applications.Services
{
    public class TipoUnidadeService
    {
        private readonly ITipoUnidadeRepository _repository;

        public TipoUnidadeService(ITipoUnidadeRepository repository)
        {
            _repository = repository;
        }

        public List<LerTipoUnidadeDto> Listar()
        {
            return _repository.Listar().Select(t => TipoUnidadeConversao.ConverterParaDto(t)).ToList();
        }

        public LerTipoUnidadeDto ObterPorNome(string nome)
        {
            LerTipoUnidadeDto unidade = TipoUnidadeConversao.ConverterParaDto(_repository.ObterPorNome(nome));

            if (unidade == null)
            {
                throw new DomainException("Tipo Unidade não encontrado.");
            }

            return unidade;
        }
    }
}
