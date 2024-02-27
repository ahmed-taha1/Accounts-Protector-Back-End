using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly IJwtService _jwtService;
        public PlatformController(IPlatformService platformService, IJwtService jwtService)
        {
            _platformService = platformService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddPlatform([FromBody] DtoAddPlatformRequest request)
        {
            // get the token from the header
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;

            if (await _platformService.AddPlatformAsync(request, userEmail))
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest();
        }
    }
}
