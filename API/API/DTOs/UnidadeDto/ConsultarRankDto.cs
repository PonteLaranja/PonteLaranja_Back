using System;

namespace API.DTOs.UnidadeDto
{
    public class ConsultarRankDto
    {
        public Guid TipoItemID { get; set; }

        // Exclusivo para o TipoItem Roupa (PP, P, M, G, GG)
        public string? Tamanho { get; set; }
    }
}