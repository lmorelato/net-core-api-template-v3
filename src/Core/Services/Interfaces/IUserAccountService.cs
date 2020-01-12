using System.Threading.Tasks;
using Template.Core.Models.Dtos;

namespace Template.Core.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task SendConfirmationEmailAsync(string userName);

        Task ConfirmEmailAsync(int userId, string token);

        Task UpdatePasswordAsync(int userId, PasswordDto passwordDto);

        Task ResetPasswordAsync(string userName);
    }
}
