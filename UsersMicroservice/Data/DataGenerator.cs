using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersMicroservice.Models;

namespace UsersMicroservice.Data
{
    public static class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UsersContext(
                serviceProvider.GetRequiredService<DbContextOptions<UsersContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // Data was already seeded
                }

                context.Users.AddRange(
                    new User
                    {
                        Id = 1,
                        Username = "Bob1965"
                    },
                    new User
                    {
                        Id = 2,
                        Username = "Alice1991"
                    });

                context.SaveChanges();
            }
        }
    }
}
