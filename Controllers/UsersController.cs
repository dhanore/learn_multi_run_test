using Microsoft.AspNetCore.Mvc;
using yoyo.web.BL.Filters;
using yoyo.web.BL.Models;
using yoyo.web.BL.Services;

namespace yoyo_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()  //IHttpResponse
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
