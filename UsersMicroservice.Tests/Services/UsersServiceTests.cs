using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersMicroservice.Data;
using UsersMicroservice.Models;
using UsersMicroservice.Services;
using Xunit;

namespace UsersMicroservice.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IUsersRepository> _mockRepo;
        private readonly UsersService _service;

        public UsersServiceTests()
        {
            _mockRepo = new Mock<IUsersRepository>();

            _service = new UsersService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateUser_ValidUsername_ReturnsSuccessServiceResponse()
        {
            // arrange
            string username = "TestUser";

            _mockRepo.Setup(r => r.DoesUsernameExist(username))
                .ReturnsAsync(false);

            // act
            ServiceResponse result = await _service.CreateUser(username);

            // assert
            Assert.Equal(ServiceResponseType.Success, result.ResponseType);
            Assert.NotNull(result.CreatedId);
            _mockRepo.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetAllUsers_NoUsersExist_ReturnsEmptyList()
        {
            // arrange
            _mockRepo.Setup(r => r.GetAllUsers())
                .ReturnsAsync(new List<User>());

            // act
            List<UserResponse> result = await _service.GetAllUsers();

            // assert
            Assert.True(result.Count == 0);
            _mockRepo.Verify(r => r.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_UsernameAlreadyExists_ReturnsErrorServiceResponse()
        {
            // arrange
            string username = "TestUser";

            _mockRepo.Setup(r => r.DoesUsernameExist(username))
                .ReturnsAsync(true);

            // act
            ServiceResponse result = await _service.UpdateUser(1, username);

            // assert
            Assert.Equal(ServiceResponseType.Conflict, result.ResponseType);
            Assert.NotNull(result.ErrorMessage);
            _mockRepo.Verify(r => r.UpdateContext(), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_UsernameNotFound_ReturnsFalse()
        {
            // arrange
            _mockRepo.Setup(r => r.GetUserWithTracking(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            // act
            bool result = await _service.DeleteUser(1);

            // assert
            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never);
        }

        // add further tests to improve code coverage
    }
}
