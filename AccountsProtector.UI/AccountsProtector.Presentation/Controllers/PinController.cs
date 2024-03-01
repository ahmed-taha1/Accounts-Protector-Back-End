using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public PinController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SetPin([FromBody] DtoSetPinRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;
            if (await _userService.SetPinAsync(request.Pin!, userEmail))
            {
                return Ok(new DtoMsgResponse{Message = "pin has been set"});
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "user is not found or pin has been set before" } });
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CheckPin([FromBody] DtoPin request)
        {
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;

            if (await _userService.CheckPinAsync(request.Pin!, userEmail))
            {
                return Ok();
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "pin is invalid" } });
        }
    }
}
