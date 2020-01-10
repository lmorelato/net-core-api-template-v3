using System;

namespace Template.Core.Models.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime Expires { get; set; }

        public string UserName { get; set; }

        public int Id { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }
    }
}
