using API.Applications.Services;
using API.DTOs.TipoItemDto;
using API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoItemController : ControllerBase
    {
        private readonly TipoItemService _service;

        public TipoItemController(TipoItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerTipoItemDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("nome/{nome}")]
        [Consumes("Multipart/form-data")]
        public ActionResult<LerTipoItemDto> ObterPorNome(string nome)
        {
            try
            {
                return Ok(_service.ObterPorNome(nome));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
