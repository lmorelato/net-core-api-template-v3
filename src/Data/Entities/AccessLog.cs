using System;
using Template.Data.Entities.Identity;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Entities
{
    public class AccessLog : IBaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string IpAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public User User { get; set; }
    }
}
