using API.Applications.Services;
using API.DTOs.EstadoTransferenciaDto;
using API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoTransferenciaController : ControllerBase
    {
        private readonly EstadoTransferenciaService _estadoTransferenciaService;

        public EstadoTransferenciaController(EstadoTransferenciaService estadoTransferenciaService)
        {
            _estadoTransferenciaService = estadoTransferenciaService;
        }

        [HttpGet]
        public ActionResult<List<LerEstadoTransferenciaDto>> Listar()
        {
            return Ok(_estadoTransferenciaService.Listar());
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<LerEstadoTransferenciaDto> ObterPorNome(string nome)
        {
            try
            {
                LerEstadoTransferenciaDto estadoTransferencia = _estadoTransferenciaService.ObterPorNome(nome);
                return Ok(estadoTransferencia);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
