using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Identity
{
    public static class ApplicationIdentityDataSeeding
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if(! userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed",
                    Email = "ahmeddddddddddd@gmail.com",
                    UserName="Ahmed",
                    PhoneNumber="010000000000000"
                };
                await userManager.CreateAsync(user,"P@ssw0rd");
            }
        }
    }
}
