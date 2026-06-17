using API.Applications.Conversoes;
using API.Applications.Regras;
using API.Domains;
using API.DTOs.UnidadeDto;
using API.Exceptions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
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

            _repository.Cadastrar(unidadeCadastrada, unidadeCadastrada.UsuarioResponsavelID, unidadeCadastrada.TipoUnidadeID);
        }

        public void Atualizar(Guid id, CadastrarUnidadeDto criarDto)
        {
            Unidade unidade = _repository.BuscarPorGuid(id);
            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            Usuario usuario = _repository.BuscarUsuario(criarDto.UsuarioResponsavelDto);
            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            TipoUnidade tipoUnidade = _repository.BuscarTipoUnidade(criarDto.TipoUnidadeDto);
            if (tipoUnidade == null)
            {
                throw new DomainException("Tipo de unidade não encontrado.");
            }

            ValidarCampo.NomeUnidade(criarDto.NomeUnidadeDto);
            ValidarCampo.CEP(criarDto.CEPDto);

            Unidade unidadeAtualizada = UnidadeConversao.CadastrarOuAtualizar(criarDto, id);
            unidadeAtualizada.UnidadeID = id;

            _repository.Atualizar(unidadeAtualizada, criarDto.UsuarioResponsavelDto, criarDto.TipoUnidadeDto);
        }

        public void Inativar(Guid unidadeId)
        {
            Unidade unidade = _repository.BuscarPorGuid(unidadeId);

            if (unidade == null)
            {
                throw new DomainException("Unidade não encontrada.");
            }

            _repository.Inativar(unidadeId);
        }

        public List<RankPrioridadeDto> ObterRankPrioridade(ConsultarRankDto consultaDto)
        {
            if (consultaDto == null)
            {
                throw new DomainException("Preencha os campos da consulta.");
            }

            // Puxa todas as unidades ativas através do repositório existente
            List<Unidade> unidades = _repository.Listar();

            // Validação estrita para identificar se é Roupa com base nos tamanhos reais do banco
            var tamanhosValidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "PP", "P", "M", "G", "GG" };
            bool ehRoupa = !string.IsNullOrEmpty(consultaDto.Tamanho) && tamanhosValidos.Contains(consultaDto.Tamanho);

            string nomeTipoItem = ehRoupa ? "Roupa" : "Item";
            string tamanhoFiltro = ehRoupa ? consultaDto.Tamanho.ToUpper() : null;
            string unidadeMedidaPadrao = ehRoupa ? $"Peças ({tamanhoFiltro})" : "Unidades";

            var listaRank = new List<RankPrioridadeDto>();
            int mesAtual = DateTime.Today.Month;

            foreach (var unidade in unidades)
            {
                // Busca o estoque total passando o filtro tratado (ou null para não-roupas)
                int estoque = _repository.ObterEstoqueAtualPorTipoItem(unidade.UnidadeID, consultaDto.TipoItemID, tamanhoFiltro);

                // Busca o total de baixas ocorridas neste mesmo mês
                int totalBaixas = _repository.ObterTotalBaixasSazonais(unidade.UnidadeID, consultaDto.TipoItemID, mesAtual, tamanhoFiltro);

                double mediaDiaria = 0.0;

                if (totalBaixas > 0)
                {
                    // TRATAMENTO DE DADOS: Descobre a idade do sistema para ponderar a média pelos anos corretos
                    int anoAtual = DateTime.Today.Year;
                    int anoInicialDoSistema = 2024; // Ano em que o sistema começou a funcionar

                    // Calcula quantos anos o sistema possui de histórico (Garante no mínimo 1 para evitar divisão por zero)
                    int quantidadeAnosHistoricos = (anoAtual - anoInicialDoSistema) + 1;

                    // O divisor comercial reflete os 30 dias multiplicados pelo número de anos acumulados no banco
                    int totalDiasHistoricosConsiderados = quantidadeAnosHistoricos * 30;

                    // Média diária real baseada em todo o histórico do mês através dos anos
                    mediaDiaria = (double)totalBaixas / totalDiasHistoricosConsiderados;
                }

                // Calcula os dias restantes de estoque
                int diasRestantes;
                if (mediaDiaria == 0)
                {
                    diasRestantes = estoque == 0 ? 0 : 999;
                }
                else
                {
                    diasRestantes = (int)Math.Ceiling(estoque / mediaDiaria);
                }

                // Define o nível de urgência baseado nos dias de cobertura
                string urgencia = "Estável";
                if (diasRestantes <= 7) urgencia = "Crítica";
                else if (diasRestantes <= 15) urgencia = "Atenção";

                listaRank.Add(new RankPrioridadeDto
                {
                    UnidadeId = unidade.UnidadeID,
                    NomeUnidade = unidade.NomeUnidade,
                    CEP = unidade.CEP,
                    TamanhoConsultado = tamanhoFiltro,
                    EstoqueAtual = estoque,
                    MediaConsumoDiarioMesAtual = Math.Round(mediaDiaria, 3),
                    DiasRestantesDeEstoque = diasRestantes,
                    NivelUrgencia = urgencia,
                    TipoItemConsultado = nomeTipoItem,
                    UnidadeMedida = unidadeMedidaPadrao
                });
            }

            // Ordena o Rank por menor tempo de estoque restante
            var rankingOrdenado = listaRank
                .OrderBy(r => r.DiasRestantesDeEstoque)
                .ToList();

            // Atribui as posições sequenciais do ranking ordenado
            for (int i = 0; i < rankingOrdenado.Count; i++)
            {
                rankingOrdenado[i].PosicaoRankingPrioridade = i + 1;
            }

            return rankingOrdenado;
        }
    }
}