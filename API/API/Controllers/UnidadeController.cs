using API.Applications.Services;
using API.DTOs.UnidadeDto;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController : ControllerBase
    {
        private readonly UnidadeService _service;

        public UnidadeController(UnidadeService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerUnidadeDto>> Listar()
        {
            List<LerUnidadeDto> unidades = _service.Listar();

            return Ok(unidades);
        }

        [HttpGet("{id}")]
        public ActionResult<LerUnidadeDto> BuscarPorGuid(Guid id)
        {
            try
            {
                LerUnidadeDto unidade = _service.BuscarPorGuid(id);
                return Ok(unidade);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("rank-prioridade")]
        public ActionResult<List<RankPrioridadeDto>> ObterRankPrioridade(ConsultarRankDto consultaDto)
        {
            try
            {
                List<RankPrioridadeDto> ranking = _service.ObterRankPrioridade(consultaDto);
                return Ok(ranking);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao processar o ranking de prioridades.");
            }
        }

        [HttpPost]
        public ActionResult Cadastrar(CadastrarUnidadeDto criarDto)
        {
            try
            {
                _service.Cadastrar(criarDto);
                return StatusCode(201);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CadastrarUnidadeDto atualizarDto)
        {
            try
            {
                _service.Atualizar(id, atualizarDto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Inativar(Guid id)
        {
            try
            {
                _service.Inativar(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}