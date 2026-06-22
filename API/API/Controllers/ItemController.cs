using API.Applications.Services;
using API.DTOs.ItemDto;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<LerItemDto>> Listar()
        {
            return Ok(_itemService.Listar());
        }

        [HttpGet("{id}")]
        public ActionResult<LerItemDto> ObterPorId(Guid id)
        {
            try
            {
                return Ok(_itemService.ObterPorId(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Responsavel,Administrador")]
        [HttpGet("listarTipo/{id}")]
        public ActionResult<List<LerItemDto>> ListarPorTipo(Guid id)
        {
            try
            {
                return Ok(_itemService.ListarPorTipo(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Responsavel,Administrador")]
        [HttpGet("listarPorUsuario/{id}")]
        public ActionResult<List<LerItemDto>> ListarPorUsuario(Guid id)
        {
            try
            {
                return Ok(_itemService.ListarPorUsuario(id));
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Responsavel,Administrador")]
        [HttpPost]
        public ActionResult Adicionar(AdicionarItemDto criarItemDto)
        {
            try
            {
                _itemService.Adicionar(criarItemDto);
                return Ok();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Responsavel,Administrador")]
        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, AtualizarItemDto atualizarItemDto)
        {
            try
            {
                _itemService.Atualizar(id, atualizarItemDto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Responsavel,Administrador")]
        [HttpDelete("{id}")]
        public ActionResult Inativar(Guid id)
        {
            try
            {
                _itemService.Inativar(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
