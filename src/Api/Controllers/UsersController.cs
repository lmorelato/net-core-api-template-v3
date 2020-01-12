using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToolbox.Extensions.Strings;
using Template.Api.Controllers.Bases;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Localization;
using Template.Shared;
using Template.Shared.Session;

namespace Template.Api.Controllers
{
    public sealed class UsersController : AuthControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserAccountService userAccountService;
        private readonly ISharedResources localizer;

        public UsersController(
            IUserService userService,
            IUserAccountService userAccountService,
            IUserSession currentUser,
            ISharedResources localizer) : base(currentUser)
        {
            this.userService = userService;
            this.userAccountService = userAccountService;
            this.localizer = localizer;
        }

        /// <summary>
        /// Find user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Found user</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            var result = await this.userService.GetAsync(id);
            return this.Ok(result);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>A newly created user</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostAsync([FromBody] CredentialsDto credentials)
        {
            var result = await this.userService.AddAsync(credentials);
            return this.CreatedAtAction(nameof(this.GetAsync), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="userDto">User data</param>
        /// <returns>No content</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UserDto userDto)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            userDto.Id = id;
            await this.userService.UpdateAsync(userDto);
            return this.NoContent();
        }

        /// <summary>
        /// Update user's culture
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>No content</returns>
        [HttpPatch("{id:int}/culture")]
        public async Task<IActionResult> UpdateCultureAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            await this.userService.UpdateCultureAsync(id);
            return this.NoContent();
        }

        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}/password")]
        public async Task<IActionResult> UpdatePasswordAsync(int id, [FromBody] PasswordDto passwordDto)
        {
            await this.userAccountService.UpdatePasswordAsync(id, passwordDto);
            return this.NoContent();
        }

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(nameof(id));
            }

            await this.userService.RemoveAsync(id);
            return this.NoContent();
        }

        /// <summary>
        /// Resend confirmation email
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("{userName}/send-confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmailAsync([FromRoute] string userName)
        {
            await this.userAccountService.SendConfirmationEmailAsync(userName);
            return this.NoContent();
        }

        /// <summary>
        /// Confirm user email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:int}/" + Constants.Api.Actions.ConfirmEmail, Name = Constants.Api.Actions.ConfirmEmail)]
        public async Task<ContentResult> ConfirmEmailAsync([FromRoute] int id, string token)
        {
            await this.userAccountService.ConfirmEmailAsync(id, token);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><body><div>" + this.localizer.Get("EmailConfirmed") + "</div></body></html>"
            };
        }

        /// <summary>
        /// Reset a user password
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("{userName}/password-reset")]
        public async Task<IActionResult> ResetPasswordAsync([FromRoute] string userName)
        {
            if (!userName.IsValidEmailAddress())
            {
                return this.NotFound(nameof(userName));
            }

            await this.userAccountService.ResetPasswordAsync(userName);
            return this.NoContent();
        }
    }
}