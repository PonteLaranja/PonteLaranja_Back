using API.Applications.Auth;
using API.Domains;
using API.DTOs.AuthDto;
using API.Interfaces;
using API.Exceptions;
using API.Applications.Conversoes;

namespace API.Applications.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AuthService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        public TokenDto Login(LoginDto loginDto)
        {

            if (string.IsNullOrWhiteSpace(loginDto.email))
                throw new DomainException("Preencha o email");

            if (string.IsNullOrWhiteSpace(loginDto.senha))
                throw new DomainException("Preencha a senha");

            Usuario? usuario = _repository.ObterPorEmail(loginDto.email);

            

            if (usuario == null)
                throw new DomainException("Usuário invalido");

            Console.WriteLine(usuario.Senha);
            if (!ConverterSenhaHash.HashSenha(loginDto.senha).SequenceEqual(usuario.Senha))
                throw new DomainException("Senha invalida");
            
            var token = _tokenJwt.GerarToken(usuario);

            return new TokenDto
            {
                token = token
            };

        }

    }
}
