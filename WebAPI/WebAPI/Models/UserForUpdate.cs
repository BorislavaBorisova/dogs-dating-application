using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for  for updating the information of a user.
    /// </summary>
    public class UserForUpdate
    {
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string SocialMediaLink { get; set; }

        public string City { get; set; }
    }
}
