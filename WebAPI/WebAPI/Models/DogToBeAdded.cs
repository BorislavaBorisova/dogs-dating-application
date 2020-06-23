using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for adding a dog.
    /// </summary>
    public class DogToBeAdded
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Breed { get; set; }

        public string Specifics { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
