using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersMicroservice.Data;
using UsersMicroservice.Models;

namespace UsersMicroservice.Services
{
    public interface IUsersService
    {
        Task<ServiceResponse> CreateUser(string username);
        Task<List<UserResponse>> GetAllUsers();
        Task<ServiceResponse> UpdateUser(int id, string username);
        Task<bool> DeleteUser(int id);
    }

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepo;

        public UsersService(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<ServiceResponse> CreateUser(string username)
        {
            bool alreadyExists = await _usersRepo.DoesUsernameExist(username);
            
            if (alreadyExists)
            {
                return new ServiceResponse
                {
                    ResponseType = ServiceResponseType.Conflict,
                    IsError = true,
                    ErrorMessage = $"Username {username} already exists"
                };
            }

            User newUser = new User
            {
                Username = username
            };

            await _usersRepo.CreateUser(newUser);

            return new ServiceResponse { ResponseType = ServiceResponseType.Success, CreatedId = newUser.Id.ToString() };
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            List<User> users = await _usersRepo.GetAllUsers();

            List<UserResponse> userResponses = new List<UserResponse>();

            userResponses = users.Select(x => new UserResponse()
            {
                UserId = x.Id,
                Username = x.Username
            }).ToList();

            return userResponses;
        }

        public async Task<ServiceResponse> UpdateUser(int id, string username)
        {
            bool alreadyExists = await _usersRepo.DoesUsernameExist(username);

            if (alreadyExists)
            {
                return new ServiceResponse
                {
                    ResponseType = ServiceResponseType.Conflict,
                    IsError = true,
                    ErrorMessage = $"Username {username} already exists"
                };
            }

            User user = await _usersRepo.GetUserWithTracking(id);

            if (user == null)
            {
                return new ServiceResponse
                {
                    ResponseType = ServiceResponseType.NotFound,
                    IsError = true,
                    ErrorMessage = $"User not found with given ID {id}"
                };
            }

            user.Username = username;

            await _usersRepo.UpdateContext();

            return new ServiceResponse { ResponseType = ServiceResponseType.Success };
        }

        public async Task<bool> DeleteUser(int id)
        {
            User user = await _usersRepo.GetUserWithTracking(id);

            if (user == null)
            {
                return false;
            }

            await _usersRepo.DeleteUser(user);

            return true;
        }
    }
}
