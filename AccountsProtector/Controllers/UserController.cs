using DataLayer.DTO;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ServicesLayer.UserService;

namespace AccountsProtector.Controllers
{
    [Route("api/[controller]/[action]")]
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
            // validating model
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errors);
            }

            User user = new User
            {
                PersonName = request.PersonName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            IdentityResult result = await _userService.Register(user, request.Password);

            // if registration failed
            if (!result.Succeeded)
            {
                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return BadRequest(errors);
            }

            // if registration succeeded
            return Ok(StatusCode(StatusCodes.Status201Created, user.Id));
        }

        [HttpGet]
        public async Task<IActionResult> IsEmailIsAlreadyRegistered(string email)
        {
            return Ok(await _userService.IsEmailIsAlreadyRegistered(email));
        }
    }
}
