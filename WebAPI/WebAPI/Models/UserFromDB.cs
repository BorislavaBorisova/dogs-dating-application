using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for user from database.
    /// </summary>
    public class UserFromDB
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string SocialMediaLink { get; set; }

        public Guid City { get; set; }
    }
}
