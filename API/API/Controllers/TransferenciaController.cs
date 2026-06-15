using API.Applications.Services;
using API.DTOs.TransferenciaDto;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciaController : ControllerBase
    {
        private readonly TransferenciaService _service;

        public TransferenciaController(TransferenciaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            return Ok(_service.Listar());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Adicionar(CriarTransferenciaDto dto)
        {
            try
            {
                string usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(usuarioIdClaim))
                {
                    return Unauthorized("Usuario não autenticado");
                }

                Guid usuarioId = Guid.Parse(usuarioIdClaim);
                _service.Adicionar(usuarioId,dto);

                return Created();
            } catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("{id}/responder")]
        public ActionResult Responder(Guid id, ResponderSolicitacaoDto resposta)
        {
            try
            {
                string usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(usuarioIdClaim))
                {
                    return Unauthorized("Usuário não autenticado");
                }

                Guid usuarioId = Guid.Parse(usuarioIdClaim);

                _service.Responder(id, usuarioId, resposta);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
