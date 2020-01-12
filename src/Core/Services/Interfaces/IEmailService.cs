using System.Threading.Tasks;

using Template.Core.Models;

namespace Template.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailOptions options, bool throwIfError = false);
    }
}