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

        [HttpDelete]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeletePlatform([FromBody] DtoDeletePlatformRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;
            if (await _platformService.DeletePlatformAsync(request.Id, userEmail))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms()
        {
            string token= Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;
            ICollection<Platform> platforms = await _platformService.GetAllPlatforms(userEmail);
            DtoGetAllPlatformsResponse response = new DtoGetAllPlatformsResponse
            {
                Platforms = platforms.Select(p => new DtoPlatform
                {
                    Id = p.Id,
                    PlatformName = p.PlatformName,
                    IconColor = p.IconColor,
                    NumOfAccounts = p.Accounts.Count
                }).ToList()
            };
            return Ok(response);
        }
    }
}