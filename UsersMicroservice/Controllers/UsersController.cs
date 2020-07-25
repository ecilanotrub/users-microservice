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
                            _logger.LogInformation($"CreateUser returned a Conflict response: {result.ErrorMessage}");
                            return Conflict(result.ErrorMessage);
                    }
                }

                _logger.LogInformation("CreateUser was called successfully");
                return CreatedAtAction(nameof(GetAllUsers), new { result.CreatedId });
            }

            catch (Exception ex)
            {
                _logger.LogInformation($"CreateUser has thrown an exception {ex}");
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
                    _logger.LogInformation("GetAllUsers was called successfully, no users found");
                    return Ok();
                }

                _logger.LogInformation("GetAllUsers was called successfully");
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"GetAllUsers has thrown an exception {ex}");
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
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int userId))
            {
                _logger.LogInformation("UpdateUser was called with an invalid user ID");
                return BadRequest("User ID was either not specified or was invalid");
            }

            try
            {
                ServiceResponse result = await _usersService.UpdateUser(userId, request.Username);

                if (result.IsError)
                {
                    switch (result.ResponseType)
                    {
                        case ServiceResponseType.Conflict:
                            _logger.LogInformation($"UpdateUser returned a Conflict response: {result.ErrorMessage}");
                            return Conflict(result.ErrorMessage);
                        case ServiceResponseType.NotFound:
                            _logger.LogInformation($"UpdateUser returned a NotFound response: {result.ErrorMessage}");
                            return NotFound(result.ErrorMessage);
                    }
                }

                _logger.LogInformation($"UpdateUser was called successfully for User ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateUser has thrown an exception {ex}");
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
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int userId))
            {
                _logger.LogInformation("DeleteUser was called with an invalid user ID");
                return BadRequest("User ID was either not specified or was invalid");
            }

            try
            {
                bool deleted = await _usersService.DeleteUser(userId);

                if (!deleted)
                {
                    _logger.LogInformation("DeleteUser returned a NotFound response");
                    return NotFound($"User ID {userId} not found");
                }

                _logger.LogInformation($"DeleteUser was called successfully for User ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteUser has thrown an exception {ex}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
