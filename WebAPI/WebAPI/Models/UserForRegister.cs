using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for user registration.
    /// </summary>
    public class UserForRegister
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password should not be shorter than 8 characters.")]
        [MaxLength(20, ErrorMessage = "Password should not be exceed 20 characters.")]
        public string Password { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password should not be shorter than 8 characters.")]
        [MaxLength(20, ErrorMessage = "Password should not be exceed 20 characters.")]
        public string RepeatedPassword { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string SocialMediaLink { get; set; }

        [Required]
        public string City { get; set; }
    }
}
