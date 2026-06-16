using API.Applications.Services;
using API.DTOs.UsuarioDto;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult<List<LerUsuarioDto>> Listar()
        {
           return Ok(_service.Listar());
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public ActionResult<LerUsuarioDto> ObterPorId(Guid id)
        {
            try
            {
                var usuario = _service.ObterPorId(id);
                return Ok(usuario);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("email/{email}")]
        public ActionResult<LerUsuarioDto> ObterPorEmail(string email)
        {
            try
            {
                var usuario = _service.ObterPorEmail(email);
                return Ok(usuario);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Criar(CriarUsuarioDto dto)
        {
            try
            {
                _service.Criar(dto);
                return Created();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, CriarUsuarioDto dto)
        {
            try
            {
                _service.Atualizar(id, dto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public ActionResult Inativar(Guid id)
        {
            try
            {
                _service.Inativar(id);
                return NoContent();
            } catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
