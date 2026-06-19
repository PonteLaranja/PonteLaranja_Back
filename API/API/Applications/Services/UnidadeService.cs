using API.Applications.Conversoes;
using API.Applications.Regras;
using API.Domains;
using API.DTOs.UnidadeDto;
using API.Exceptions;
using API.Interfaces;
using API.Repositories;
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

            // 1. Puxa todas as unidades ativas mapeadas no banco
            var unidades = _repository.Listar();

            // 2. Busca o nome real do TipoItem dinamicamente
            string nomeTipoItem = _repository.ObterNomeTipoItemPorId(consultaDto.TipoItemID) ?? "Item";

            // 3. Define dinamicamente a unidade de medida com base no tipo real e tamanho
            string unidadeMedidaPadrao = DefinirUnidadeMedida(nomeTipoItem, consultaDto.Tamanho);
            string? tamanhoFiltro = !string.IsNullOrEmpty(consultaDto.Tamanho) ? consultaDto.Tamanho.ToUpper() : null;

            var listaRank = new List<RankPrioridadeDto>();

            // Variáveis temporais para o cálculo preciso do mês vigente
            DateTime hoje = DateTime.Today;
            int mesAtual = hoje.Month;
            int diaAtual = hoje.Day; // Quantos dias se passaram no mês atual

            foreach (var unidade in unidades)
            {
                // Busca estoque (com o cálculo de Kg/Litro corrigido no Repository)
                int estoque = _repository.ObterEstoqueAtualPorTipoItem(unidade.UnidadeID, consultaDto.TipoItemID, tamanhoFiltro);

                // Busca o total de baixas (doações reais) efetuadas neste mês
                int totalBaixas = _repository.ObterTotalBaixasSazonais(unidade.UnidadeID, consultaDto.TipoItemID, mesAtual, tamanhoFiltro);

                // Média diária real baseada nos dias decorridos do mês atual para manter a proporção correta
                double mediaDiaria = totalBaixas > 0 ? (double)totalBaixas / diaAtual : 0.0;

                // Calcula os dias restantes de estoque tratando o colapso (estoque zero)
                int diasRestantes;
                if (estoque == 0)
                {
                    diasRestantes = 0; // Se não tem nada, a cobertura é imediata (zero dias)
                }
                else if (mediaDiaria == 0)
                {
                    diasRestantes = 999; // Tem estoque mas não há consumo registrado: estável
                }
                else
                {
                    diasRestantes = (int)Math.Ceiling(estoque / mediaDiaria);
                }

                // Define o nível de urgência
                string urgencia = "Estável";
                if (estoque == 0 || diasRestantes <= 7) urgencia = "Crítica";
                else if (diasRestantes <= 15) urgencia = "Atenção";

                listaRank.Add(new RankPrioridadeDto
                {
                    UnidadeId = unidade.UnidadeID,
                    NomeUnidade = unidade.NomeUnidade,
                    CEP = unidade.CEP,
                    TipoItemConsultado = nomeTipoItem,
                    TamanhoConsultado = tamanhoFiltro,
                    UnidadeMedida = unidadeMedidaPadrao,
                    EstoqueAtual = estoque,
                    MediaConsumoDiarioMesAtual = Math.Round(mediaDiaria, 3),
                    DiasRestantesDeEstoque = diasRestantes,
                    NivelUrgencia = urgencia
                });
            }

            // ORDENAÇÃO INTELIGENTE:
            // Primeiro: Critica > Atencao > Estavel
            // Segundo (Desempate): Menor quantidade de dias restantes de estoque vai para o topo
            var rankingOrdenado = listaRank
                .OrderBy(r => r.NivelUrgencia == "Crítica" ? 1 : r.NivelUrgencia == "Atenção" ? 2 : 3)
                .ThenBy(r => r.DiasRestantesDeEstoque)
                .ToList();

            // Atribui as posições sequenciais no topo do ranking ordenado (1, 2, 3...)
            for (int i = 0; i < rankingOrdenado.Count; i++)
            {
                rankingOrdenado[i].PosicaoRankingPrioridade = i + 1;
            }

            return rankingOrdenado;
        }

        private string DefinirUnidadeMedida(string tipoItem, string? tamanho)
        {
            if (tipoItem.Equals("Alimento", StringComparison.OrdinalIgnoreCase)) return "Kg";
            if (tipoItem.Equals("Bebida", StringComparison.OrdinalIgnoreCase)) return "Litros";
            if (tipoItem.Equals("Roupa", StringComparison.OrdinalIgnoreCase))
                return string.IsNullOrEmpty(tamanho) ? "Peças" : $"Peças ({tamanho.ToUpper()})";

            return "Unidades"; // Para brinquedos ou genéricos
        }
    }
}