using System;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for dog from database.
    /// </summary>
    public class DogFromDB
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public Guid Breed { get; set; }

        public Guid Owner { get; set; }

        public string Specifics { get; set; }

        public string ProfilePicturePath { get; set; }

        public Guid City { get; set; }
    }
}
