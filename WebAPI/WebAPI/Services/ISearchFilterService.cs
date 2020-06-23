using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    /// <summary>
    /// Represents a set of methods for search and filter in database tables.
    /// </summary>
    public interface ISearchFilterService
    {
        /// <summary>
        /// Gets all cities names from database begining with the given letters.
        /// </summary>
        /// <returns>
        /// Returns a list of cities names.
        /// </returns>
        /// <param name="letters"></param>
        List<dynamic> GetCities(string letters);

        /// <summary>
        /// Gets all breeds names from database begining with the given letters.
        /// </summary>
        /// <returns>
        /// Returns a list of cities names.
        /// </returns>
        /// <param name="letters"></param>
        List<dynamic> GetBreeds(string letters);
    }
}
