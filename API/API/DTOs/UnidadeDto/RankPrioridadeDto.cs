using System;

namespace API.DTOs.UnidadeDto
{
    public class RankPrioridadeDto
    {
        public int PosicaoRankingPrioridade { get; set; }

        public string NivelUrgencia { get; set; } = string.Empty;

        public Guid UnidadeId { get; set; }

        public string NomeUnidade { get; set; } = string.Empty;

        public string CEP { get; set; } = string.Empty;

        public string TipoItemConsultado { get; set; } = string.Empty;

        // Ex: "M" se for Roupa, ou null para os demais tipos
        public string? TamanhoConsultado { get; set; }

        // Ex: "Kg", "Litros", "Peças"
        public string UnidadeMedida { get; set; } = string.Empty;

        public int EstoqueAtual { get; set; }

        // Quantidade média calculada baseada nas baixas históricas do mês vigente
        public double MediaConsumoDiarioMesAtual { get; set; }

        public int DiasRestantesDeEstoque { get; set; }
    }
}