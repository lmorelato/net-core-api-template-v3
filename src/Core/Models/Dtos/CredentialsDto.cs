using Template.Core.Models.Dtos.Bases;

namespace Template.Core.Models.Dtos
{
    public sealed class PasswordDto : BaseDto
    {
        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}
