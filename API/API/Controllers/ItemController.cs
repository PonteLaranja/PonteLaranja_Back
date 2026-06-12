using API.Applications.Services;
using API.DTOs.ItemDto;
using API.Exceptions;
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

        [HttpGet("listarTipo/{id}")]
        public ActionResult<List<LerItemDto>> ListarPorTipo(Guid id)
        {
            try
            {
                return Ok(_itemService.ListarPorTipo(id));
            } catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Adicionar(AdicionarItemDto criarItemDto)
        {
            try
            {
                _itemService.Adicionar(criarItemDto);
                return Ok();
            } catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(Guid id, AtualizarItemDto atualizarItemDto)
        {
            try
            {
                _itemService.Atualizar(id, atualizarItemDto);
                return NoContent();
            } catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Deletar(Guid id)
        {
            try
            {
                _itemService.Deletar(id);
                return NoContent();
            } catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
