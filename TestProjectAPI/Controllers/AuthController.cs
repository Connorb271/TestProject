using Microsoft.AspNetCore.Mvc;
using TestProjectAPI.Models;
using TestProjectAPI.Services;

namespace TestProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = await _loginService.LoginAsync(model.Email, model.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
