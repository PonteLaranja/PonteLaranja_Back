using API.Applications.Conversoes;
using API.Applications.Regras;
using API.Domains;
using API.DTOs.UnidadeDto;
using API.Exceptions;
using API.Interfaces;
using System.Security.Cryptography.Xml;

namespace API.Applications.Services
{
    public class UnidadeService
    {
        private readonly IUnidadeRepository _repository;

        public UnidadeService(IUnidadeRepository repository)
        {
            _repository = repository;
        }

        public List<LerUnidadeDto> Listar()
        {
            List<Unidade> unidades = _repository.Listar();

            List<LerUnidadeDto> dtos = unidades
                .Select(UnidadeConversao.LerDto).ToList();

            return dtos;
        }

        public LerUnidadeDto BuscarPorGuid(Guid id)
        {
            if (id == null)
            {
                throw new DomainException("Informe o ID da unidade.");
            }

            Unidade unidade = _repository.BuscarPorGuid(id);

            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            return UnidadeConversao.LerDto(unidade);
        }

        public LerUnidadeDto BuscarPorNome(string nomeUnidade)
        {
            if (nomeUnidade == null)
            {
                throw new DomainException("Informe o nome da unidade.");
            }

            Unidade unidade = _repository.BuscarPorNome(nomeUnidade);

            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            return UnidadeConversao.LerDto(unidade);
        }

        public LerUnidadeDto BuscarPorCEP(string cep)
        {
            if (cep == null)
            {
                throw new DomainException("Informe o CEP da unidade.");
            }

            Unidade unidade = _repository.BuscarPorCEP(cep);

            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            return UnidadeConversao.LerDto(unidade);
        }

        public void Cadastrar(CadastrarUnidadeDto criarDto)
        {
            Usuario usuario = _repository.BuscarUsuario(criarDto.UsuarioResponsavelDto);
            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            TipoUnidade tipoUnidade = _repository.BuscarTipoUnidade(criarDto.TipoUnidadeDto);
            if (tipoUnidade == null)
            {
                throw new DomainException("Tipo de unidade não encontrada.");
            }

            ValidarCampo.NomeUnidade(criarDto.NomeUnidadeDto);
            ValidarCampo.CEP(criarDto.CEPDto);

            Unidade unidadeCadastrada = UnidadeConversao.CadastrarOuAtualizar(criarDto);

            _repository.Cadastrar(unidadeCadastrada);
        }

        public void Atualizar(Guid id, CadastrarUnidadeDto criarDto)
        {
            CadastrarOuAtualizarUnidadeDto unidade = BuscarPorGuid(unidadeAtualizada.UnidadeID);
            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada ou inexistente.");
            }

            Usuario usuario = _repository.BuscarUsuario(responsavelId);
            if (usuario == null)
            {
                return;
            }

            TipoUnidade tipoUnidade = _repository.BuscarTipoUnidade(tipoUnidadeId);
            if (tipoUnidade == null)
            {
                return;
            }

            ValidarCampo.NomeUnidade(unidadeAtualizada.NomeUnidade);
            ValidarCampo.CEP(unidadeAtualizada.CEP);

            _repository.Atualizar(unidadeAtualizada, responsavelId, tipoUnidadeId);
        }

        public void Deletar(Guid unidadeId)
        {
            Unidade unidade = _repository.BuscarPorGuid(unidadeId);

            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            _repository.Deletar(unidadeId);
        }
    }
}