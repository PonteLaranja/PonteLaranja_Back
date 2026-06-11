using API.Applications.Conversoes;
using API.DTOs.UsuarioDto;
using API.Interfaces;
using API.Exceptions;

namespace API.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly ITipoUsuarioRepository _repositoryTipoUsuario;

        public UsuarioService(IUsuarioRepository repositoryUsuario, ITipoUsuarioRepository repositoryTipoUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryTipoUsuario = repositoryTipoUsuario;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email invalido");
            }
        }

        public List<LerUsuarioDto> Listar()
        {
            return _repositoryUsuario.Listar().Select(u => UsuarioConversao.ConverterParaDto(u)).ToList();
        }

        public LerUsuarioDto ObterPorId(Guid id)
        {
            var usuario = _repositoryUsuario.ObterPorId(id);
            if (usuario == null)
            {
                throw new DomainException("Usuario não encontrado");
            }
            return UsuarioConversao.ConverterParaDto(usuario);
        }
        public LerUsuarioDto ObterPorEmail(string email)
        {
            var usuario = _repositoryUsuario.ObterPorEmail(email);
            if (usuario == null)
            {
                throw new DomainException("Email não encontrado.");
            }
            return UsuarioConversao.ConverterParaDto(usuario);
        }

        public void Criar(CriarUsuarioDto criarDto)
        {
            bool tipoUsuarioExiste = _repositoryTipoUsuario.TipoUsuarioExiste(criarDto.TipoUsuarioID);

            ValidarEmail(criarDto.Email);

            if (!tipoUsuarioExiste)
            {
                throw new DomainException("Tipo Usuario não encontrado.");
            }

            bool usuarioEmail = _repositoryUsuario.EmailExiste(criarDto.Email);

            if (usuarioEmail)
            {
                throw new DomainException("Email ja cadastrado");
            }

            _repositoryUsuario.Cadastrar(UsuarioConversao.DtoParaDomains(criarDto));
        }
        public void Atualizar(Guid id, CriarUsuarioDto atualizarDto)
        {
            var usuarioExistente = _repositoryUsuario.ObterPorId(id);
            if (usuarioExistente == null)
            {
                throw new DomainException("Usuario não encontrado.");
            }

            ValidarEmail(atualizarDto.Email);

            bool usuarioEmail = _repositoryUsuario.EmailExiste(atualizarDto.Email);
            if (usuarioEmail)
            {
                throw new DomainException("Email ja cadastrado");
            }

            bool tipoUsuarioExiste = _repositoryTipoUsuario.TipoUsuarioExiste(atualizarDto.TipoUsuarioID);

            if (!tipoUsuarioExiste)
            {
                throw new DomainException("Tipo Usuario não encontrado.");
            }

            usuarioExistente.Nome = atualizarDto.Nome;
            usuarioExistente.Email = atualizarDto.Email;
            usuarioExistente.TipoUsuarioID = atualizarDto.TipoUsuarioID;

            _repositoryUsuario.Atualizar(id, usuarioExistente);
        }
    }
}
