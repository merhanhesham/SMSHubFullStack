using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities.Identity;

namespace SMSHub.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        
        //seed user into db
        public static async Task SeedUserAsync(UserManager<AppUser>userManager, RoleManager<IdentityRole> roleManager)
        {
            //seed roles
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            if (!await roleManager.RoleExistsAsync("MarketingManager"))
            {
                await roleManager.CreateAsync(new IdentityRole("MarketingManager"));
            }
            if (!await roleManager.RoleExistsAsync("CustomerSupport"))
            {
                await roleManager.CreateAsync(new IdentityRole("CustomerSupport"));
            }

            
            //seed users
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "MerhanHesham",
                    Email = "merhan100.hesham@gmail.com",
                    UserName = "merhanhesham",
                    PhoneNumber = "01003106587"
                    
                };
                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            // Find the user by their email or another unique identifier
            var existingUser = await userManager.FindByEmailAsync("merhan100.hesham@gmail.com");

            if (existingUser != null)
            {
                // Check if the user is already in the "admin" role
                if (!await userManager.IsInRoleAsync(existingUser, "admin"))
                {
                    // Add the user to the "admin" role
                    await userManager.AddToRoleAsync(existingUser, "admin");
                }
            }
            


        }
    }
}
