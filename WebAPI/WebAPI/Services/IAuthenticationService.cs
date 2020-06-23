using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    /// <summary>
    /// Represents a set of methods for user authentication.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userForRegister"></param>
        void Register(UserForRegister userForRegister);

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <returns>
        /// Returns a JWT assosiated with the user if loged in successfully.
        /// </returns>
        /// <param name="userForLogIn"></param>
        string LogIn(UserForLogIn userForLogIn);
    }
}
