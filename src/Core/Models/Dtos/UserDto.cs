using Template.Core.Models.Dtos.Bases;

namespace Template.Core.Models.Dtos
{
    public sealed class UserDto : BaseDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string FullName { get; set; }

        public string Culture { get; set; }
    }
}
