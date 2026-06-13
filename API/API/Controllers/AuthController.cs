using API.Applications.Services;
using API.DTOs.AuthDto;
using API.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        

        private readonly AuthService _service;
        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<TokenDto> Login(LoginDto login)
        {
            try
            {
                return Ok(_service.Login(login));

            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
