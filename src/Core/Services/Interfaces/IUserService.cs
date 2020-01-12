using System.Threading.Tasks;
using Template.Core.Models.Dtos;

namespace Template.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddAsync(CredentialsDto credentials);

        Task UpdateAsync(UserDto userDto);

        Task UpdateCultureAsync(int userId);

        Task RemoveAsync(int userId);

        Task<UserDto> GetAsync(int userId);
    }
}
