using API.Applications.Services;
using API.DTOs.TipoUsuarioDto;
using API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        public readonly TipoUsuarioService _service;

        public TipoUsuarioController(TipoUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerTipoUsuarioDto>> Listar()
        {
            try
            {
                return Ok(_service.Listar());
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<LerTipoUsuarioDto> ObterPorId(Guid id)
        {
            try
            {
                var tipo = _service.ObterPorId(id);
                return Ok(tipo);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<LerTipoUsuarioDto> ObterPorNome(string nome)
        {
            try
            {
                var tipo = _service.ObterPorNome(nome);
                return Ok(tipo);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
