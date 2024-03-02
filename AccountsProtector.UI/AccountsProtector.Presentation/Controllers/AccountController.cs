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
                DtoAccountId id = new DtoAccountId
                {
                    AccountId = accountId,
                };
                return StatusCode(StatusCodes.Status201Created, id);
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "platform is not found" } });
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAccountsByPlatformId([FromBody] DtoGetAccountsByPlatformIdRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            DtoGetAccountsByPlatformIdResponse? response = new DtoGetAccountsByPlatformIdResponse();
            response.Accounts = await _accountService.GetAccountsByPlatformIdAsync(request.PlatformId, userId);
            if (response.Accounts != null)
            {
                return Ok(response);
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "platform is not found" } });
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAccountById([FromBody] DtoAccountId request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            DtoAccount? account = await _accountService.GetAccountByIdAsync((int)request.AccountId!, userId);
            if (account != null)
            {
                return Ok(account);
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "account is not found" } });
        }

        [HttpDelete]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeleteAccount([FromBody] DtoAccountId request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            if (await _accountService.DeleteAccountAsync((int)request.AccountId!, userId))
            {
                return Ok();
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "account is not found" } });
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAccount([FromBody] DtoUpdateAccountRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            if (await _accountService.UpdateAccountAsync(request, userId))
            {
                return Ok();
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "account is not found" } });
        }
    }
}
