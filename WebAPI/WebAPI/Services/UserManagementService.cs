using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebAPI.Exceptions;
using WebAPI.Models;

namespace WebAPI.Services
{
    /// <summary>
    /// Represents a set of methods for user management.
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private string _connectionString;

        private string _defaultProfilePicture;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="UserManagementService"/> class.
        ///  </summary>
        /// <param name="configuration"></param>
        public UserManagementService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
#else
            _connectionString = configuration.GetConnectionString("Production");
            
#endif
            _defaultProfilePicture = configuration.GetValue<string>("DefaultProfilePicture");
        }

        ///<summary>
        ///Implements IUserManagemetService.GetUserById
        ///</summary>
        public UserForDisplay GetUserById(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM [USER] WHERE Id = @Id";
                var userFromDb = connection.QuerySingleOrDefault<UserFromDB>(query, new
                {
                    Id = id
                });

                var result = new UserForDisplay();
                result.FullName = userFromDb.FullName;
                result.Username = userFromDb.Username;
                result.PhoneNumber = userFromDb.PhoneNumber;
                result.Email = userFromDb.Email;
                result.SocialMediaLink = userFromDb.SocialMediaLink;
                result.City = GetCityById(connection, userFromDb.City);

                return result;
            }
        }

        ///<summary>
        ///Implements IUserManagemetService.Update
        ///</summary>
        public void UpdateUser(Guid id, UserForUpdate userForUpdate)
        {
            if (userForUpdate.Equals(null))
            {
                throw new ArgumentNullException();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var ownerQuery = @"UPDATE [User]
                               SET PhoneNumber = @PhoneNumber, Email = @Email, SocialMediaLink = @SocialMediaLink, City = @City
                               WHERE Id = @Id;";

                var dogQuery = @"UPDATE Dog
                                SET City = @City
                                WHERE Owner = @Owner";

                var cityId = GetCityId(connection, userForUpdate.City);

                connection.Execute(ownerQuery, new
                {
                    PhoneNumber = userForUpdate.PhoneNumber,
                    Email = userForUpdate.Email,
                    SocialMediaLink = userForUpdate.SocialMediaLink,
                    City = cityId,
                    Id = id
                });

                connection.Execute(dogQuery, new
                {
                    City = cityId,
                    Owner = id
                });
            }
        }

        ///<summary>
        ///Implements IUserManagemetService.AddDog
        ///</summary>
        public void AddDog(DogToBeAdded dogToBeAdded, Guid ownerId)
        {
            if (dogToBeAdded.Equals(null))
            {
                throw new ArgumentNullException();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"INSERT INTO [Dog] (Id, Name, Age, Gender, Breed, Owner, Specifics, ProfilePicturePath, City) 
                              VALUES (newid(), @Name, @Age, @Gender, @Breed, @Owner, @Specifics, @ProfilePicturePath, @City)";

                var cityId = GetOwnersCityId(connection, ownerId);

                if (dogToBeAdded.ProfilePicturePath == "")
                {
                    dogToBeAdded.ProfilePicturePath = _defaultProfilePicture;
                }

                var rowsAffected = connection.Execute(query, new
                {
                    Name = dogToBeAdded.Name,
                    Age = dogToBeAdded.Age,
                    Gender = dogToBeAdded.Gender,
                    Breed = GetBreedId(connection, dogToBeAdded.Breed),
                    Owner = ownerId,
                    Specifics = dogToBeAdded.Specifics,
                    ProfilePicturePath = dogToBeAdded.ProfilePicturePath,
                    City = cityId
                });
            }

        }

        ///<summary>
        ///Implements IUserManagemetService.UpdateDog
        ///</summary>
        public void UpdateDog(Guid dogId, DogForUpdate dogForUpdate)
        {
            if (dogForUpdate.Equals(null))
            {
                throw new ArgumentNullException();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"UPDATE Dog
                            SET Name = @Name, Age = @Age, Specifics = @Specifics, ProfilePicturePath = @ProfilePicturePath
                            WHERE Id = @Id;";

                connection.Execute(query, new
                {
                    Name = dogForUpdate.Name,
                    Age = dogForUpdate.Age,
                    Specifics = dogForUpdate.Specifics,
                    ProfilePicturePath = dogForUpdate.ProfilePicturePath,
                    Id = dogId
                });
            }
        }

        ///<summary>
        ///Implements IUserManagemetService.GetDogById
        ///</summary>
        public DogFromDB GetDogById(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Dog WHERE [Id] = @Id";

                return connection.QueryFirstOrDefault<DogFromDB>(query, new { Id = id });
            }
        }

        ///<summary>
        ///Implements IUserManagemetService.GetAllDogs
        ///</summary>
        public List<DogForDisplayWithId> GetAllDogs(int page, Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT  * FROM  Dog
                        WHERE (NOT Owner=@Owner)
                        ORDER BY [Name]
                        OFFSET (@Page-1)*4 ROWS
                        FETCH NEXT 4 ROWS ONLY";

                var dogsFromDB = connection.Query<DogFromDB>(query, new { Page = page, Owner = currentOwnerId }).ToList();

                var result = new List<DogForDisplayWithId>();

                foreach (var item in dogsFromDB)
                {
                    var dog = new DogForDisplayWithId();
                    dog.Id = item.Id;
                    dog.Name = item.Name;
                    dog.Age = item.Age;
                    dog.Gender = item.Gender;
                    dog.Breed = GetBreedById(connection, item.Breed);
                    dog.Owner = item.Owner;
                    dog.Specifics = item.Specifics;
                    dog.ProfilePicturePath = item.ProfilePicturePath;

                    result.Add(dog);
                }

                return result;
            }
        }

        ///<summary>
        ///Implements IUserManagemetService.FilterDogs
        ///</summary>
        public List<DogForDisplayWithId> FilterDogs(int page, Filters filters, Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string cityId = string.Empty;
                string breedId = string.Empty;

                if (!string.IsNullOrEmpty(filters.Breed))
                {
                    breedId = GetBreedId(connection, filters.Breed).ToString();
                }

                if (string.IsNullOrEmpty(filters.Gender))
                {
                    filters.Gender = string.Empty;
                }

                if (!string.IsNullOrEmpty(filters.City))
                {
                    cityId = GetCityId(connection, filters.City).ToString();
                }

                if ((filters.MinAge == 0 && filters.MaxAge == 0) || (filters.MinAge != 0 && filters.MaxAge == 0))
                {
                    filters.MaxAge = 30;
                }

                string query = @"SELECT  * FROM  Dog
                        WHERE (Age BETWEEN @MinAge and @MaxAge) AND (Breed LIKE '%' + @Breed ) AND (Gender LIKE '%' + @Gender ) AND (City LIKE '%' + @City) AND (NOT Owner=@Owner)
                        ORDER BY [Name]
                        OFFSET (@Page-1)*4 ROWS
                        FETCH NEXT 4 ROWS ONLY";

                var filteredDogs = connection.Query<DogFromDB>(query, new
                {
                    MinAge = filters.MinAge,
                    MaxAge = filters.MaxAge,
                    Breed = breedId,
                    Gender = filters.Gender,
                    City = cityId,
                    Owner = currentOwnerId,
                    Page = page
                }).ToList();

                if (filteredDogs.Count == 0)
                {
                    throw new NoEntriesException();
                }

                var result = new List<DogForDisplayWithId>();

                foreach (var item in filteredDogs)
                {
                    var dog = new DogForDisplayWithId();
                    dog.Id = item.Id;
                    dog.Name = item.Name;
                    dog.Age = item.Age;
                    dog.Gender = item.Gender;
                    dog.Breed = GetBreedById(connection, item.Breed);
                    dog.Owner = item.Owner;
                    dog.Specifics = item.Specifics;
                    dog.ProfilePicturePath = item.ProfilePicturePath;

                    result.Add(dog);
                }

                return result;
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.GetDogEntriesCount
        /// </summary>
        public int GetDogEntriesCount(Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT COUNT(*) FROM Dog 
                              WHERE (NOT Owner=@Owner)";

                return connection.QueryFirstOrDefault<int>(query, new { Owner = currentOwnerId });
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.GetFilteredDogsCount
        /// </summary>
        public int GetFilteredDogsCount(Filters filters, Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string cityId = string.Empty;
                string breedId = string.Empty;

                if (!string.IsNullOrEmpty(filters.Breed))
                {
                    breedId = GetBreedId(connection, filters.Breed).ToString();
                }

                if (string.IsNullOrEmpty(filters.Gender))
                {
                    filters.Gender = string.Empty;
                }

                if (!string.IsNullOrEmpty(filters.City))
                {
                    cityId = GetCityId(connection, filters.City).ToString();
                }

                if ((filters.MinAge == 0 && filters.MaxAge == 0) || (filters.MinAge != 0 && filters.MaxAge == 0))
                {
                    filters.MaxAge = 30;
                }

                string query = @"SELECT COUNT(*) FROM Dog
                        WHERE (Age BETWEEN @MinAge and @MaxAge) AND (Breed LIKE '%' + @Breed ) AND (Gender LIKE '%' + @Gender ) AND (City LIKE '%' + @City) AND (NOT Owner=@Owner)";

                var filteredDogsCount = connection.QueryFirstOrDefault<int>(query, new
                {
                    MinAge = filters.MinAge,
                    MaxAge = filters.MaxAge,
                    Breed = breedId,
                    Gender = filters.Gender,
                    City = cityId,
                    Owner = currentOwnerId
                });

                return filteredDogsCount;
            }
        }

        ///<summary>Implements IUserManagemetService.GetOwnersDogs</summary>
        public List<DogForDisplayWithId> GetOwnersDogs(Guid ownersId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Dog WHERE Owner = @Id";

                var dogs = connection.Query<DogFromDB>(query, new { Id = ownersId }).ToList();

                var result = new List<DogForDisplayWithId>();

                foreach (var item in dogs)
                {
                    var dog = new DogForDisplayWithId();
                    dog.Id = item.Id;
                    dog.Name = item.Name;
                    dog.Age = item.Age;
                    dog.Gender = item.Gender;
                    dog.Breed = GetBreedById(connection, item.Breed);
                    dog.Owner = item.Owner;
                    dog.Specifics = item.Specifics;
                    dog.ProfilePicturePath = item.ProfilePicturePath;

                    result.Add(dog);
                }

                return result;
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.DeleteDog
        /// </summary>
        public void DeleteDog(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM [Dog] WHERE [Id] = @Id";

                connection.Execute(query, new { Id = id });
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.GetLikedDogs
        /// </summary>
        public List<DogForDisplay> GetLikedDogs(int page, Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT  Dog_ID2 FROM  LikedDogs
                        WHERE User_ID1=@Owner
                        ORDER BY [User_ID1]
                        OFFSET (@Page-1)*4 ROWS
                        FETCH NEXT 4 ROWS ONLY";

                var likedDogsGuids = connection.Query<Guid>(query, new
                {
                    Page = page,
                    Owner = currentOwnerId
                }).ToList();

                List<DogForDisplay> likedDogs = new List<DogForDisplay>();

                foreach (var guid in likedDogsGuids)
                {
                    DogForDisplayWithId dogForDisplay = new DogForDisplayWithId();
                    string queryForOneDog = @"SELECT  * FROM  Dog
                        WHERE Id=@Id";

                    var dog = connection.QueryFirstOrDefault<DogFromDB>(queryForOneDog, new { Id = guid });

                    dogForDisplay.Age = dog.Age;
                    dogForDisplay.Breed = GetBreedById(connection, dog.Breed);
                    dogForDisplay.Id = dog.Id;
                    dogForDisplay.Gender = dog.Gender;
                    dogForDisplay.Name = dog.Name;
                    dogForDisplay.Owner = dog.Owner;
                    dogForDisplay.ProfilePicturePath = dog.ProfilePicturePath;
                    dogForDisplay.Specifics = dog.Specifics;

                    likedDogs.Add(dogForDisplay);
                }

                return likedDogs;
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.GetLikedDogsCount
        /// </summary>
        public int GetLikedDogsCount(Guid currentOwnerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT COUNT(*) FROM LikedDogs 
                              WHERE User_ID1=@OwnerId";

                return connection.QueryFirstOrDefault<int>(query, new { OwnerId = currentOwnerId });
            }
        }

        /// <summary>
        /// Implements IUserManagemetService.LikeDog
        /// </summary>
        public void LikeDog(Guid currentOwnerId, Guid dogId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"INSERT INTO [LikedDogs] (User_ID1, Dog_ID2) 
                              VALUES (@User, @Dog)";
                      
                var rowsAffected = connection.Execute(query, new
                {
                    User = currentOwnerId,
                    Dog = dogId
                });
            }
        }

        private Guid GetCityId(SqlConnection connection, string city)
        {
            var query = "SELECT ID FROM [City] WHERE Name = @Name";

            return connection.QueryFirstOrDefault<Guid>(query, new { Name = city });
        }

        private Guid GetBreedId(SqlConnection connection, string breedName)
        {
            var query = "SELECT [Id] FROM Breed WHERE Name = @BreedName";

            return connection.QueryFirstOrDefault<Guid>(query, new { BreedName = breedName });
        }

        private string GetCityById(SqlConnection connection, Guid id)
        {
            var query = "SELECT Name FROM [City] WHERE Id = @Id";

            return connection.QueryFirstOrDefault<string>(query, new { Id = id });
        }

        private string GetBreedById(SqlConnection connection, Guid id)
        {
            var query = "SELECT Name FROM [Breed] WHERE Id = @Id";

            return connection.QueryFirstOrDefault<string>(query, new { Id = id });
        }

        private Guid GetOwnersCityId(SqlConnection connection, Guid id)
        {
            var query = @"SELECT City FROM [User] WHERE @Id = id";
            return connection.QueryFirstOrDefault<Guid>(query, new
            {
                Id = id
            });
        }
    }
}

