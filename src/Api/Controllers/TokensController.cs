using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Api.Controllers.Bases;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Shared.Session;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : AppControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly IUserSession userSession;

        public TokensController(ITokenService tokenService, IUserSession userSession)
        {
            this.tokenService = tokenService;
            this.userSession = userSession;
        }

        [HttpPost]
        public async Task<ActionResult<TokenDto>> AuthenticateAsync([FromBody]CredentialsDto credentials)
        {
            this.userSession.DisableTenantFilter = true;
            var result = await this.tokenService.AuthenticateAsync(credentials);
            return result;
        }
    }
}