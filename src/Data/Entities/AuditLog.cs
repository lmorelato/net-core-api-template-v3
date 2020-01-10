using System;

using Microsoft.EntityFrameworkCore;

namespace Template.Data.Entities
{
    public class AuditLog : AutoHistory
    {
        public DateTime ModifiedOn { get; set; }

        public int ModifiedById { get; set; }
    }
}
