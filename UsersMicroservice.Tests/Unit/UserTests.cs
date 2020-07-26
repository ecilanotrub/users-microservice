using System;
using System.Collections.Generic;
using System.Text;
using UsersMicroservice.Models;
using Xunit;

namespace UsersMicroservice.Tests.Unit
{
    public class UserTests
    {
        [Fact]
        public void PropertyUsername_SuccessfullyGetsAndSets()
        {
            string username = "regina_phalange";

            User user = new User 
            {
                Username = username
            };
            
            Assert.Equal(username, user.Username);
        }
    }
}
