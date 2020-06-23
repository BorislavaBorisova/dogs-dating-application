using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class SearchFilterService : ISearchFilterService
    {
        private string _connectionString;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="SearchFilterService"/> class.
        ///  </summary>
        /// <param name="configuration"></param>
      public SearchFilterService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
#else
            _connectionString = configuration.GetConnectionString("Production");
#endif
        }

        ///<summary>Implements ISearchFilterService.GetCities</summary>
        /// <param name="letters"></param>
        public List<dynamic> GetCities(string letters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Name FROM City WHERE (Name LIKE @Letters + '%')";

                var result = connection.Query(query, new { Letters = letters }).ToList();

                return result;
            }
        }

        ///<summary>Implements ISearchFilterService.GetBreeds</summary>
        /// <param name="letters"></param>
        public List<dynamic> GetBreeds(string letters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Name FROM Breed WHERE (Name LIKE @Letters + '%')";

                var result = connection.Query(query, new { Letters = letters }).ToList();

                return result;
            }
        }
    }
}
