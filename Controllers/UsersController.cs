using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using UsersMicroservice.Models;
using UsersMicroservice.Services;

namespace UsersMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(ILogger<UsersController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, type: typeof(CreatedAtActionResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                ServiceResponse result = await _usersService.CreateUser(request.Username);

                if (result.IsError)
                {
                    switch (result.ResponseType)
                    {
                        case ServiceResponseType.Conflict:
                            // log conflict
                            return Conflict(result.ErrorMessage);
                    }
                }

                // log successful create
                return CreatedAtAction(nameof(GetAllUsers), new { result.CreatedId });
            }

            catch (Exception)
            {
                // log exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a list of users
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(List<UserResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<UserResponse> responses = await _usersService.GetAllUsers();

                if (responses == null || responses.Count == 0)
                {
                    // log success but none found
                    return Ok();
                }

                // log: GetAllUsers was called successfully
                return Ok(responses);
            }
            catch (Exception)
            {
                // log exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates the username of a user for the given ID
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UserRequest request)
        {
            if (string.IsNullOrEmpty(id))
            {
                // log bad request
                return BadRequest("User ID must be specified");
            }

            try
            {
                await _usersService.UpdateUser(id, request.Username);

                return NoContent();
            }
            catch (Exception)
            {
                // log exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // log bad request
                return BadRequest("User ID must be specified");
            }

            try
            {
                bool deleted = await _usersService.DeleteUser(id);

                if (!deleted)
                {
                    // log not found
                    return NotFound();
                }

                // log successful delete
                return NoContent();
            }
            catch (Exception)
            {
                // log exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
