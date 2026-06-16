using API.Applications.Auth;
using API.Domains;
using API.DTOs.UsuarioDto;

namespace API.Applications.Conversoes
{
    public class UsuarioConversao
    {
        public static LerUsuarioDto ConverterParaDto(Usuario usuario)
        {
            return new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoUsuarioID = usuario.TipoUsuarioID,
            };
        }

        public static Usuario DtoParaDomains(CriarUsuarioDto usuario)
        {
            return new Usuario {
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoUsuarioID = usuario.TipoUsuarioID,
                Senha = CriptografarUsuario.HashSenha(usuario.Senha),
            };
        }
    }
}
