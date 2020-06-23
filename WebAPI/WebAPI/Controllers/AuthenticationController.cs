using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Provides functionality to the /Authentication/ route.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="AuthenticationController"/> class.
        ///  </summary>
        /// <param name="authenticationService"></param>
        public AuthenticationController(Services.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userForRegister"></param>
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserForRegister userForRegister)//
        {
            try
            {
                _authenticationService.Register(userForRegister);
                return Ok(new { Success = true });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = invalidOperationException.Message
                });
            }
            catch(ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                }) ;
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,//
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// /// <param name="userForLogIn"></param>
        [HttpPost("login")]
        public IActionResult LogIn([FromBody]UserForLogIn userForLogIn)
        {
            try
            {
                var tokenString = _authenticationService.LogIn(userForLogIn);
                return Ok(new
                {
                    Success = true,
                    Token = tokenString
                });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = invalidOperationException.Message
                });
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = argumentNullException.Message
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}