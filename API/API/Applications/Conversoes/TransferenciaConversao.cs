using API.Domains;
using API.DTOs.TransferenciaDto;

namespace API.Applications.Conversoes
{
    public class TransferenciaConversao
    {
        public static LerTransferenciaDto ConverterParaDto(Transferencia dto)
        {
            return new LerTransferenciaDto
            {
                TransferenciaID = dto.TransferenciaID,
                ItemID = dto.ItemID,
                ItemCategoria = dto.Item.TipoItem.NomeTipoItem,
                DataChegada = dto.DataChegada,
                DataEnvio = dto.DataEnvio,
                EstadoTransferenciaID = dto.EstadoTransferenciaID,
                UnidadeDestinoID = dto.UnidadeDestinoID,
                UnidadeDestinoNome = dto.UnidadeDestino.NomeUnidade,
                UnidadeOrigemID = dto.UnidadeOrigemID,
                UnidadeOrigemNome = dto.UnidadeOrigem.NomeUnidade
            };
        }

        public static Transferencia ConverterParaDomain(CriarTransferenciaDto dto)
        {
            return new Transferencia
            {
                ItemID = dto.ItemID,
                DataChegada = dto.DataChegada,
                DataEnvio = dto.DataEnvio,
                EstadoTransferenciaID = dto.EstadoTransferenciaID,
                UnidadeDestinoID = dto.UnidadeDestinoID,
                UnidadeOrigemID = dto.UnidadeOrigemID,
            };
        }

        private static readonly HashSet<string> TamanhosDeRoupa = new(StringComparer.OrdinalIgnoreCase) { "PP", "P", "M", "G", "GG" };

        public static LogDoacaoDto MapearParaLogDto(Transferencia transferencia)
        {
            string nomeTipoMedida = transferencia.Item?.TipoMedida?.NomeTipoMedida ?? "Nulo";
            bool ehTamanhoRoupa = TamanhosDeRoupa.Contains(nomeTipoMedida);

            // Se o doador não tiver e-mail cadastrado, é um doador externo da rua
            var usuarioDoador = transferencia.Item?.Usuario;
            bool ehDoadorExterno = usuarioDoador == null || string.IsNullOrEmpty(usuarioDoador.Email);

            string fase = ehDoadorExterno ? "Recebimento" : "Baixa";
            string nomeUnidadeExibicao = ehDoadorExterno ? "Doação externa" : (transferencia.UnidadeDestino?.NomeUnidade ?? "Unidade Não Identificada");

            return new LogDoacaoDto
            {
                LogItemId = transferencia.TransferenciaID,
                ItemId = transferencia.ItemID,
                NomeItem = transferencia.Item?.NomeItem ?? "Item Não Identificado",
                UsuarioDoador = usuarioDoador != null ? usuarioDoador.Nome : "Doador Anônimo",
                Unidade = nomeUnidadeExibicao, // Exibe "Doação externa" ou o nome da unidade de destino
                Quantidade = transferencia.Item?.Quantidade ?? 0,
                Medida = transferencia.Item?.Medida ?? 0,
                TipoMedida = ehTamanhoRoupa ? "Peças" : nomeTipoMedida,
                Tamanho = ehTamanhoRoupa ? nomeTipoMedida.ToUpper() : null,
                FaseDoacao = fase,
                Data = transferencia.DataEnvio ?? DateTime.Now
            };
        }
    }
}
