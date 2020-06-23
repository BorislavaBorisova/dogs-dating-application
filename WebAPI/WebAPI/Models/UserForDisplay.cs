using System;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for displaying user.
    /// </summary>
    public class UserForDisplay
    {
        public string FullName { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string SocialMediaLink { get; set; }

        public string City { get; set; }
    }
}
