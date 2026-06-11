using API.Applications.Services;
using API.DTOs.TipoMedidaDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMedidaController : ControllerBase
    {
        private readonly TipoMedidaService _service;

        public TipoMedidaController(TipoMedidaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerTipoMedidaDto>> Listar()
        {
            return Ok(_service.Listar());
        }

        public ActionResult<LerTipoMedidaDto> ObterPorNome(string nome)
        {
            try
            {
                LerTipoMedidaDto medida = _service.ObterPorNome(nome);
                return Ok(medida);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
