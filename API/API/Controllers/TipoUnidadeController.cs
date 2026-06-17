using API.Applications.Services;
using API.DTOs.TipoUnidadeDto;
using API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUnidadeController : ControllerBase
    {
        private readonly TipoUnidadeService _service;

        public TipoUnidadeController(TipoUnidadeService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerTipoUnidadeDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<LerTipoUnidadeDto> ObterPorNome(string nome)
        {
            try
            {
                return Ok(_service.ObterPorNome(nome));
            }catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
