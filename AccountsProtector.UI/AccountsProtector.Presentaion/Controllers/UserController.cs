using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentaion.Controllers
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
        public async Task<IActionResult> Register([FromBody] DtoRegisterUser request)
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] DtoUserLoginRequest request, [FromServices] IConfiguration configuration)
        {
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

            User? user = await _userService.GetUserByEmail(request.Email);

            if (user == null || !await _userService.Login(user.Email, request.Password))
            {
                return BadRequest("Invalid email or password");
            }

            DtoUserLoginResponse response = new DtoUserLoginResponse
            {
                Token = _jwtService.GenerateToken(user),
                Email = user.Email,
                PersonName = user.PersonName,
                Expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT:EXPIRY_IN_DAYS"]))
            };
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] DtoUserChangePassword request)
        {
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

            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string token = authorizationHeader.Split(' ').LastOrDefault();
            string email = _jwtService.GetEmailFromToken(token);

            if (email == null || !await _userService.UpdatePassword(request.OldPassword, request.NewPassword, email))
            {
                return BadRequest("Invalid Data");

            }
            return Ok("Password Changed Successfully");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendOTP([FromBody] DtoSendOTPRequest request, [FromServices] IEmailService otpService)
        {
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

            User? user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest("Invalid Email");
            }

            if (await otpService.SendOTP(request.Email))
            {
                return Ok("mail has been sent");
            }
            return BadRequest("mail has not been sent");
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] DtoForgetPasswordRequest request,
            [FromServices] IEmailService otpService)
        {
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

            if (await otpService.VerifyOTP(request.Email, request.OTPCode))
            {
                if (await _userService.UpdatePassword(request.NewPassword, request.Email))
                {
                    return Ok("Password Changed Successfully");
                }
                return BadRequest("Password Change Failed");
            }
            return BadRequest("OTP invalid or expired");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailIsAlreadyRegistered(string email)
        {
            return Ok(await _userService.GetUserByEmail(email) == null);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateToken(string token, [FromServices] IJwtService jwtService)
        {
            return Ok(jwtService.ValidateToken(token));
        }
    }
}
