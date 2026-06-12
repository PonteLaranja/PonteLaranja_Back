using API.Exceptions;
using System.Reflection.Metadata.Ecma335;

namespace API.Applications.Regras
{
    public static class ValidarCampo
    {
        // O THIS possibilita o POLIMORFISMO DE EXTENSÃO
        private static void Validar(this string nome, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException(mensagem);
            }
        }

        // CAMPO GENÉRICO
        public static void Nome(this string nome) =>
            nome.Validar("O campo é obrigatório.");

        // --------------------- UNIDADE ----------------------
        public static void NomeUnidade(this string nomeUnidade) =>
            nomeUnidade.Validar("O nome da unidade é obrigatório.");

        public static void CEP(this string CEP) =>
            CEP.Validar("O CEP é obrigatório.");
    }
}
