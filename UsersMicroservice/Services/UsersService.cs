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
        Task UpdateUser(string id, string username);
        Task<bool> DeleteUser(string id);
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
            bool result = await _usersRepo.DoesUsernameExist(username);
            // return conflict if username already exists

            throw new NotImplementedException();
        }

        public Task<List<UserResponse>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUser(string id, string username)
        {
            bool result = await _usersRepo.DoesUsernameExist(username);
            // return conflict if username already exists

            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
