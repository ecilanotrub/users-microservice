using System;
using System.Collections.Generic;
using System.Text;
using UsersMicroservice.Models;

namespace UsersMicroservice.Tests._Shared
{
    internal static class TestData
    {
        internal static UserRequest GetUserRequest()
        {
            return new UserRequest
            {
                Username = "TestUser"
            };
        }

        internal static List<UserResponse> GetUserResponses()
        {
            return new List<UserResponse>
            {
                new UserResponse
                {
                    UserId = 1,
                    Username = "TestUser"
                },
                new UserResponse
                {
                    UserId = 2,
                    Username = "TestUser2"
                }
            };
        }
    }
}
