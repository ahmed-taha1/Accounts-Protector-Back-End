using DataLayer.DTO;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ServicesLayer.UserService;

namespace AccountsProtector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] DTORegisterUser request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            User user = new User
            {
                UserName = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };
            try
            {
                IdentityResult result = await _userService.Register(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Invalid data");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("User created successfully");
        }
    }
}
