using System.ComponentModel.DataAnnotations;
using Template.Core.Models.Dtos.Bases;
using Template.Localization.Resources;

namespace Template.Core.Models.Dtos
{
    public sealed class CredentialsDto : BaseDto
    {
        [Display(ResourceType = typeof(DisplayResources), Name = "Username")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(DisplayResources), Name = "Password")]
        public string Password { get; set; }
    }
}
