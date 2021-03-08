using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yoyo_web_app.Filters;
using yoyo_web_app.Models;
using yoyo_web_app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Authenticate(AuthenticateRequest model) //IHttpResponse
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
