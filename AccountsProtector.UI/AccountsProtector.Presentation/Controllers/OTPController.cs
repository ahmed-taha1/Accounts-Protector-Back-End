using System.Diagnostics;
using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        public OTPController(IUserService userService, IJwtService jwtService, IEmailService emailService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SendOTP([FromBody] DtoSendOTPRequest request)
        {
            User? user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        errors = new List<string> { "Email not found" }
                    });
            }
            if (await _emailService.SendOTP(request.Email))
            {
                return Ok();
            }
            return BadRequest(
                new DtoErrorsResponse
                {
                    errors = new List<string> { "OTP sending failed" }
                });
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> VerifyOTP([FromBody] DtoVerifyOTPRequest request)
        {
            User? user = await _userService.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        errors = new List<string> { "Email not found" }
                    });
            }

            if (await _emailService.VerifyOTP(request.Email, request.OTPCode ?? -1))
            {
                var response = new DtoVerifyOTPResponse
                {
                    Token = _jwtService.GenerateToken(user, DateTime.UtcNow.AddHours(1))
                };
                return Ok(response);
            }
            return BadRequest(
                new DtoErrorsResponse
                {
                    errors = new List<string> { "Otp is invalid or expired" }
                });
        }
    }
}
