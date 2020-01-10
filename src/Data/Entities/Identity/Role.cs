using Microsoft.AspNetCore.Identity;

namespace Template.Data.Entities.Identity
{
    public sealed class Role : IdentityRole<int>
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }
    }
}