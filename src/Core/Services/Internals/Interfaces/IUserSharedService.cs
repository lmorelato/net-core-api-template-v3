using System.Threading.Tasks;
using Template.Data.Entities.Identity;

namespace Template.Core.Services.Internals.Interfaces
{
    public interface IUserSharedService
    {
        Task<User> FindAsync(int userId);
        Task<User> FindByEmailAsync(string email);
    }
}