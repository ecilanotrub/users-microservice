using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersMicroservice.Controllers;
using UsersMicroservice.Models;
using UsersMicroservice.Services;
using UsersMicroservice.Tests._Shared;
using Xunit;

namespace UsersMicroservice.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly Mock<IUsersService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockService = new Mock<IUsersService>();

            _controller = new UsersController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task CreateUser_ServicesReturnsSuccess_ReturnsCreatedResponse()
        {
            // arrange
            _mockService.Setup(s => s.CreateUser(It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse { ResponseType = ServiceResponseType.Success});
                
            // act
            IActionResult response = await _controller.CreateUser(TestData.GetUserRequest());
            CreatedAtActionResult objectResponse = response as CreatedAtActionResult;

            // assert         
            Assert.NotNull(objectResponse.Value);
        }

        [Fact]
        public async Task GetAllUsers_ServicesReturnsList_ReturnsListOfUserResponses()
        {
            // arrange
            List<UserResponse> expectedList = TestData.GetUserResponses();

            _mockService.Setup(s => s.GetAllUsers())
                .ReturnsAsync(expectedList);

            // act
            IActionResult response = await _controller.GetAllUsers();
            OkObjectResult objectResponse = response as OkObjectResult;

            // assert         
            Assert.Equal(JsonConvert.SerializeObject(expectedList) , JsonConvert.SerializeObject(objectResponse.Value));
        }

        [Fact]
        public async Task UpdateUser_ServicesReturnsConflict_ReturnsConflictResponse()
        {
            // arrange
            _mockService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse { IsError = true, ResponseType = ServiceResponseType.Conflict, ErrorMessage = "ERROR" });

            // act
            IActionResult response = await _controller.UpdateUser("1", TestData.GetUserRequest());
            ConflictObjectResult objectResponse = response as ConflictObjectResult;

            // assert         
            Assert.NotNull(objectResponse.Value);
        }

        [Fact]
        public async Task DeleteUser_ServicesReturnsNotFound_ReturnsConflictResponse()
        {
            // arrange
            _mockService.Setup(s => s.DeleteUser(It.IsAny<int>()))
                .ReturnsAsync(false);

            // act
            IActionResult response = await _controller.DeleteUser("1");
            NotFoundObjectResult objectResponse = response as NotFoundObjectResult;

            // assert         
            Assert.NotNull(objectResponse.Value);
        }

        // add further tests to improve code coverage
    }
}
