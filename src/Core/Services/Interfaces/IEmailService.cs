using System.Threading.Tasks;

using Template.Core.Models;

namespace Template.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailSettings settings, bool throwIfError = false);
    }
}