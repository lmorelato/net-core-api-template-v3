using System.Collections.Generic;

namespace Template.Shared.Session
{
    public class UserSession : IUserSession
    {
        public int UserId { get; set; }

        public int? TenantId { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public string UserName { get; set; }

        public string IpAddress { get; set; }

        public bool DisableTenantFilter { get; set; }

        public bool DisableSoftDeleteFilter { get; set; }

        public bool IsInRole(string role) => (this.Roles ?? new List<string>()).Contains(role);
    }
}
