﻿using System.Reflection.Metadata.Ecma335;
using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.Helpers;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] DtoRegisterUser request)
        {
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
                ErrorHelper.IdentityResultErrorHandler(result);
            }

            // if registration succeeded
            return Ok(StatusCode(StatusCodes.Status201Created, user.Id));
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] DtoUserLoginRequest request, [FromServices] IConfiguration configuration)
        {
            User? user = await _userService.GetUserByEmail(request.Email);

            if (user == null || !await _userService.Login(user.Email, request.Password))
            {
                return BadRequest(new DtoErrorsResponse
                {
                    Errors = new List<string>{"User name or password invalid"}
                });
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangePassword([FromBody] DtoUserChangePassword request)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string token = authorizationHeader.Split(' ').LastOrDefault();
            string email = _jwtService.GetEmailFromToken(token);

            if (email == null || !await _userService.UpdatePassword(request.OldPassword, request.NewPassword, email))
            {
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        Errors = new List<string> { "Password Change Failed"}
                    });
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SendOTP([FromBody] DtoSendOTPRequest request, [FromServices] IEmailService otpService)
        {
            User? user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        Errors = new List<string> { "Email not found" }
                    });
            }

            if (await otpService.SendOTP(request.Email))
            {
                return Ok();
            }
            return BadRequest(
                new DtoErrorsResponse
                {
                    Errors = new List<string> { "OTP sending failed" }
                });
        }

        [HttpPut]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ForgetPassword([FromBody] DtoForgetPasswordRequest request,
            [FromServices] IEmailService otpService)
        {
            if (await otpService.VerifyOTP(request.Email, request.OTPCode))
            {
                if (await _userService.UpdatePassword(request.NewPassword, request.Email))
                {
                    return Ok();
                }
                return BadRequest(
                    new DtoErrorsResponse
                    {
                        Errors = new List<string>{ "Password Change Failed" } 

                    });
            }
            return BadRequest(
                new DtoErrorsResponse
                {
                    Errors = new List<string> { "Otp is invalid or expired" }
                }); ;
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