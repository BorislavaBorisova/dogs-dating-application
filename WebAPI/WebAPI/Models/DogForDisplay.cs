using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DogForDisplay
    {
        /// <summary>
        /// Represents a model for displaying dog information.
        /// </summary>

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Breed { get; set; }

        public Guid Owner { get; set; }

        public string Specifics { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
