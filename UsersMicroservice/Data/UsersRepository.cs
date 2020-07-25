using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersMicroservice.Models;

namespace UsersMicroservice.Data
{
    public interface IUsersRepository
    {
        Task CreateUser(EF_User user);
        Task<List<EF_User>> GetAllUsers();
        Task<EF_User> GetUserWithTracking(int userId);
        Task UpdateContext();
        Task DeleteUser(EF_User user);
        Task<bool> DoesUsernameExist(string username);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly UsersContext _context;

        public UsersRepository(UsersContext context)
        {
            _context = context;
        }

        public async Task CreateUser(EF_User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EF_User>> GetAllUsers()
        {
            List<EF_User> users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<EF_User> GetUserWithTracking(int userId)
        {
            EF_User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task UpdateContext()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(EF_User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesUsernameExist(string username)
        {
            bool result = await _context.Users
                .AnyAsync(u => u.Username == username);

            return result;
        }
    }
}
