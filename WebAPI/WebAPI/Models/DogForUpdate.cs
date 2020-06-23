using System;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for updating the information of a dog.
    /// </summary>
    public class DogForUpdate
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Specifics { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
