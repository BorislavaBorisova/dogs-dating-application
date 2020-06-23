using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Services
{
    /// <summary>
    /// Represents a set of methods for user authentication.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private string _connectionString;

        private string _secretKey;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="AuthenticationService"/> class.
        ///  </summary>
        /// <param name="configuration"></param>
        public AuthenticationService(IConfiguration configuration)
        {
#if DEBUG
            _connectionString = configuration.GetConnectionString("Development");
#else
            _connectionString = configuration.GetConnectionString("Production");
#endif
            _secretKey = configuration.GetValue<string>("SecretKey");
        }

        ///<summary>Implements IAuthenticationnService.Register</summary>
        /// <exception cref="InvalidOperationException">
        /// Throw when unable to register user.
        /// </exception>

        public void Register(UserForRegister userForRegister)
        {
            if (userForRegister.Equals(null))
            {
                throw new ArgumentNullException();
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                if (!IsUsernameUnique(connection, userForRegister.Username))
                {
                    throw new InvalidOperationException("Could not register user. Username already exists.");
                }

                if (!DoPasswordsMatch(userForRegister.Password, userForRegister.RepeatedPassword))
                {
                    throw new InvalidOperationException("Could not register user. Passwords don't match.");
                }

                string hashedPassword = HashPassword(userForRegister.Password);
                Guid cityId = GetCityId(connection, userForRegister.City);

                if (cityId == default)
                {
                    throw new InvalidOperationException("Choose a valid city.");
                }

                var query = @"INSERT INTO [User] (Id, FullName, Username, Password, PhoneNumber, Email, SocialMediaLink, City) 
                              VALUES (newid(), @FullName, @Username, @Password, @PhoneNumber, @Email, @SocialMediaLink, @City )";

                connection.Query(query, new
                {
                    FullName = userForRegister.FullName,
                    Username = userForRegister.Username,
                    Password = hashedPassword,
                    PhoneNumber = userForRegister.PhoneNumber,
                    Email = userForRegister.Email,
                    SocialMediaLink = userForRegister.SocialMediaLink,
                    City = cityId
                });
            }
        }

        ///<summary>Implements IAuthenticationnService.LogIn</summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when login failed.
        /// </exception>
        public string LogIn(UserForLogIn userForLogIn)
        {
            if (userForLogIn.Equals(null))
            {
                throw new ArgumentNullException();
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string hashedPassword = connection.QueryFirstOrDefault<string>("Select Password From [User] Where Username = @Username;", new { Username = userForLogIn.Username });

                if (hashedPassword == null)
                {
                    throw new InvalidOperationException("Login failed: No such user.");
                }
                if (!BCrypt.Net.BCrypt.Verify(userForLogIn.Password, hashedPassword))
                {
                    throw new InvalidOperationException("Login failed: Wrong password.");
                }

                var id = connection.QueryFirstOrDefault<Guid>("Select Id From [User] Where Username = @Username;", new { Username = userForLogIn.Username });

                return GenerateToken(userForLogIn, id);
            }
        }

        private bool IsUsernameUnique(SqlConnection connection, string username)
        {
            var query = "SELECT Username FROM [User] WHERE Username = @Username";

            return connection.QueryFirstOrDefault<string>(query, new { Username = username }) == null;
        }

        private bool DoPasswordsMatch(string password, string repeatedPassword)
        {
            if (password.Equals(repeatedPassword))
            {
                return true;
            }
            return false;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private Guid GetCityId(SqlConnection connection, string city)
        {
            var query = "SELECT ID FROM [City] WHERE Name = @Name";

            return connection.QueryFirstOrDefault<Guid>(query, new { Name = city });
        }

        private string GenerateToken(UserForLogIn userForLogIn, Guid id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, userForLogIn.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(420),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
