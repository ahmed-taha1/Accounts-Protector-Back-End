using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateAccount([FromBody] DtoCreateAccountRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            int accountId = await _accountService.CreateAccountAsync(request, userId);
            if (accountId != -1)
            {
                DtoCreateAccountResponse response = new DtoCreateAccountResponse
                {
                    AccountId = accountId,
                };
                return StatusCode(StatusCodes.Status201Created, response);
            }
            return BadRequest();
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAccountsByPlatformId([FromBody] DtoGetAccountsByPlatformIdRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            DtoGetAccountsByPlatformIdResponse? response =
                await _accountService.GetAccountsByPlatformIdAsync(request.PlatformId, userId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
