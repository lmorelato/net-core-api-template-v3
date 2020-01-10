using System.Threading.Tasks;

using Template.Core.Models.Dtos;

namespace Template.Core.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> AuthenticateAsync(CredentialsDto credentials);
    }
}
