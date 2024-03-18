using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.Helpers;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] DtoRegisterRequest request)
        {
            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            IdentityResult result = await _userService.RegisterAsync(user, request.Password!);

            // if registration failed
            if (!result.Succeeded)
            {
                return BadRequest(ErrorHelper.IdentityResultErrorHandler(result));
            }

            // if registration succeeded
            var token = _jwtService.GenerateToken(user, null);
            var response = new DtoRegisterResponse
            {
                Token = token
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] DtoLoginRequest request, [FromServices] IConfiguration configuration)
        {
            User? user = await _userService.GetUserByEmailAsync(request.Email!);

            if (user == null)
            {
                return BadRequest(new DtoErrorsResponse
                {
                    errors = new List<string>{"email is not exist"}
                });
            }

            if (!await _userService.LoginAsync(user.Email!, request.Password!))
            {
                return BadRequest(new DtoErrorsResponse
                {
                    errors = new List<string> { "wrong password" }
                });
            }

            DtoLoginResponse response = new DtoLoginResponse
            {
                Token = _jwtService.GenerateToken(user, null),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT:EXPIRY_IN_DAYS"])),
                PinHash = user.PinHash
            };
            return Ok(response);
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangePassword([FromBody] DtoChangePasswordRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            string? email = _jwtService.GetEmailFromToken(token!);

            if (email == null || !await _userService.UpdatePasswordAsync(request.OldPassword!, request.NewPassword!, email))
            {
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        errors = new List<string> { "Password Change Failed" }
                    });
            }
            return Ok();
        }


        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ResetPassword([FromBody] DtoResetPasswordRequest request)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
            string token = authorizationHeader.Split(' ').LastOrDefault()!;
            string email = _jwtService.GetEmailFromToken(token)!;

            if (await _userService.UpdatePasswordAsync(request.NewPassword!, email))
            {
                return Ok();
            }
            return BadRequest(
                new DtoErrorsResponse
                {
                    errors = new List<string>{ "Password Change Failed tip: \"try enter stronger password\"" }
                });
        }

        [HttpDelete]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RemoveAccount([FromBody] DtoRemoveAccountRequest request)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
            string token = authorizationHeader.Split(' ').LastOrDefault()!;
            string email = _jwtService.GetEmailFromToken(token)!;

            if (await _userService.RemoveAccountAsync(request.Password!, email))
            {
                return Ok();
            }
            return BadRequest(
                new DtoErrorsResponse 
                { 
                    errors = new List<string> { "wrong password" }
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPersonalData()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
            string email = _jwtService.GetEmailFromToken(token)!;
            User? user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                DtoGetUserPersonalDataResponse response = new DtoGetUserPersonalDataResponse
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                };
                return Ok(response);
            }
            return BadRequest(new DtoErrorsResponse
            {
                errors = new List<string> { "User not found" }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserData([FromServices] IPlatformService platformService, [FromServices] IAccountService accountService)
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()!;
            string email = _jwtService.GetEmailFromToken(token)!;
            User? user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                DtoGetAllUserDataResponse response = new DtoGetAllUserDataResponse
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
                ICollection<Platform?>? platforms = await platformService.GetAllPlatforms(user.Id.ToString());
                response.Platforms = platforms?.Select(p => new DtoPlatformWithAccounts
                {
                    PlatformId = p.Id,
                    PlatformName = p.PlatformName,
                    IconColor = p.IconColor,
                    NumOfAccounts = p.Accounts!.Count
                }).ToList();
                foreach (var platform in response.Platforms!)
                {
                    platform.Accounts = await accountService.GetAccountsByPlatformIdAsync(platform.PlatformId, user.Id.ToString());
                }
                return Ok(response);
            }
            return BadRequest(new DtoErrorsResponse
            {
                errors = new List<string> { "User not found" }
            });
        }
        // [HttpPost]
        // [AllowAnonymous]
        // public async Task<IActionResult> IsEmailIsAlreadyRegistered(string email)
        // {
        //     return Ok(await _userService.GetUserByEmailAsync(email) == null);
        // }
        //
        // [HttpPost]
        // [AllowAnonymous]
        // public IActionResult ValidateToken(string token, [FromServices] IJwtService jwtService)
        // {
        //     return Ok(jwtService.ValidateToken(token));
        // }
    }
}
