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
    /// Provides functionality to the /SearchFilter/ route.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchFilterController : ControllerBase
    {
        private ISearchFilterService _searchFilterService;

        ///  <summary>
        ///  Initializes a new instance of the <see cref="SearchFilterController"/> class.
        ///  </summary>
        /// <param name="searchFilterService"></param>
        public SearchFilterController(ISearchFilterService searchFilterService)
        {
            _searchFilterService = searchFilterService;
        }

        /// <summary>
        /// Gets all cities names from database begining with the given letters.
        /// </summary>
        /// <param name="letters"></param>
        [HttpGet("cities/{letters}")]
        public IActionResult GetCities(string letters)
        {
            try
            {
                var cities = _searchFilterService.GetCities(letters);
                return Ok(new
                {
                    Success = true,
                    Cities = cities
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
        /// Gets all breeds from database begining with the given letters.
        /// </summary>
        /// <param name="letters"></param>
        [HttpGet("breeds/{letters}")]
        public IActionResult GetBreeds(string letters)
        {
            try
            {
                var breeds = _searchFilterService.GetBreeds(letters);
                return Ok(new
                {
                    Success = true,
                    Breeds = breeds
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
    }
}