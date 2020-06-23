using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;
using Microsoft.AspNet.Identity;
using WebAPI.Exceptions;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Provides functionality to the /UserManagement/ route.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private IUserManagementService _userManagementService;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="UserManagementController"/> class.
        ///  </summary>
        /// <param name="userManagementService"></param>
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Gets current user.
        /// </summary>
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            try
            {
                var user = _userManagementService.GetUserById(Guid.Parse(User.Identity.GetUserId()));

                return Ok(user);
            }

            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("user/{id}")]
        public IActionResult GetUserById(Guid id)
        {
            try
            {
                var user = _userManagementService.GetUserById(id);

                return Ok(user);
            }

            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Updates the information of the current user.
        /// </summary>
        /// <param name="userForUpdate"></param>
        [HttpPut("update")]
        public IActionResult Update([FromBody] UserForUpdate userForUpdate)
        {
            try
            {
                _userManagementService.UpdateUser(Guid.Parse(User.Identity.GetUserId()), userForUpdate);

                return Ok(new { Success = true });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Adds a new dog.
        /// </summary>
        /// <param name="dogToBeAdded">The dog that is going to be added.</param>
        [HttpPost]
        public IActionResult Add([FromBody] DogToBeAdded dogToBeAdded)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _userManagementService.AddDog(dogToBeAdded, Guid.Parse(userId));

                return Ok(new { Success = true });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Updates the information of the dog assosiated with the given id.
        /// </summary>
        /// <param name="id">Id of the dog.</param>
        /// <param name="dogForUpdate">An instance of the <see cref="DogForUpdate"/> class that keeps the new data.</param>
        [HttpPut("update/{id}")]
        public IActionResult Update(Guid id, DogForUpdate dogForUpdate)
        {
            try
            {
                _userManagementService.UpdateDog(id, dogForUpdate);

                return Ok(new { Success = true });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets dog by id.
        /// </summary>
        /// <param name="id">Id of the dog.</param>
        [HttpGet("{id}")]
        public IActionResult GetDog(Guid id)
        {
            try
            {
                var dogFromDB = _userManagementService.GetDogById(id);

                return Ok(dogFromDB);
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets the next dogs from database for the given page.
        /// </summary>
        ///<param name = "page" > Page number.</param>
        [HttpGet("alldogs/{page}")]
        public IActionResult GetAllDogs(int page)
        {
            try
            {
                var allDogs = _userManagementService.GetAllDogs(page, Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    AllDogs = allDogs
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets filtered dogs from database for the given page.
        /// </summary>
        /// <param name="page">Page number.</param>
        ///<param name="filters">>An instance of the <see cref="Filters".</param>
        [HttpPost("filter/{page}")]
        public IActionResult FilterDogs([FromBody]Filters filters, [FromRoute]int page)
        {
            try
            {
                var filteredDogs = _userManagementService.FilterDogs(page, filters, Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    FilteredDogs = filteredDogs
                });
            }
            catch (NoEntriesException noEntriesException)
            {
                return new NoContentResult();
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets dogs of current user.
        /// </summary>
        [HttpGet("dogs")]
        public IActionResult GetOwnersDogs()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var dogs = _userManagementService.GetOwnersDogs(Guid.Parse(userId));

                return Ok(new
                {
                    Success = true,
                    Dogs = dogs
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Finds the total number of dogs in the system.
        /// </summary>
        [HttpGet("entriescount")]
        public IActionResult GetDogEntriesCount()
        {
            try
            {
                var dogEntriesCount = _userManagementService.GetDogEntriesCount(Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    Count = dogEntriesCount
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        ///  Finds the total number of dogs in the system returned by a filtering function.
        /// </summary>
        ///<param name="filters">>An instance of the <see cref="Filters".</param>
        [HttpPost("filtereddogscount")]
        public IActionResult GetFilteredDogsCount([FromBody] Filters filters)
        {
            try
            {
                var filteredDogsCount = _userManagementService.GetFilteredDogsCount(filters, Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    Count = filteredDogsCount
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Deletes a dog from the database.
        /// </summary>
        ///<param name="id">Id of dog for delete.</param>
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDog(Guid id)
        {
            try
            {
                _userManagementService.DeleteDog(id);

                return Ok(new
                {
                    Success = true
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Gets liked dogs of current owner.
        /// </summary>
        /// <param name="page">Page number.</param>
        [HttpGet("likeddogs/{page}")]
        public IActionResult GetLikedDogs([FromRoute]int page)
        {
            try
            {
                var likedDogs = _userManagementService.GetLikedDogs(page, Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    LikedDogs = likedDogs
                });
            }
            catch (NoEntriesException noEntriesException)
            {
                return new NoContentResult();
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        ///  Finds the total number of liked dogs of current owner.
        /// </summary>
        [HttpGet("likeddogscount")]        
        public IActionResult GetLikedDogsCount()
        {
            try
            {
                var likedDogsCount = _userManagementService.GetLikedDogsCount(Guid.Parse(User.Identity.GetUserId()));

                return Ok(new
                {
                    Success = true,
                    Count = likedDogsCount
                });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        ///  Adds dog and current owner to bridge table of liked dogs.
        /// </summary>
        [HttpPost("likedog/{id}")]
        public IActionResult LikeDog(Guid id)
        {
            try
            {
                _userManagementService.LikeDog(Guid.Parse(User.Identity.GetUserId()), id);

                return Ok(new { Success = true });
            }
            catch (Exception exception)
            {
                return new ObjectResult(new
                {
                    Error = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}