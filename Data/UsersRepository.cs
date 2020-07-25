using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersMicroservice.Data
{
    public interface IUsersRepository
    {
        Task<bool> DoesUsernameExist(string username);
    }

    public class UsersRepository : IUsersRepository
    {
        public Task<bool> DoesUsernameExist(string username)
        {
            throw new NotImplementedException();
        }
    }
}
