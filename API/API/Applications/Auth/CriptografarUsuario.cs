using API.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace API.Applications.Auth
{
    public class CriptografarUsuario
    {
        public static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatoria");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }
    }
}
