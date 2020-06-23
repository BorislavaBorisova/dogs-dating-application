using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    /// <summary>
    /// Represents a model for dogs filters.
    /// </summary>
    public class Filters
    {
        public string Breed { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public string Gender { get; set; }

        public string City { get; set; }
    }
}
