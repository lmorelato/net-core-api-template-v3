using System.Collections.Generic;

namespace Template.Shared.Session
{
    public interface IUserSession
    {
        int UserId { get; set; }

        int? TenantId { get; set; }

        List<string> Roles { get; set; }

        string UserName { get; set; }

        string IpAddress { get; set; }

        bool DisableTenantFilter { get; set; }

        bool DisableSoftDeleteFilter { get; set; }

        bool IsInRole(string role);
    }
}
