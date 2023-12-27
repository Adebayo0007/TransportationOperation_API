using Microsoft.AspNetCore.Identity;
using PTS_CORE.Domain.Entities;

namespace PTS_API.Utils
{
    public class DBInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DBInitializer(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task SeedInitialData()
        {
            //SEED CHAIRMAN
            if (!await _roleManager.RoleExistsAsync("Chairman"))
            {
                var chairmanRole = new ApplicationRole { Name = "Chairman", Description = "In charge everything" };
                await _roleManager.CreateAsync(chairmanRole);

                if (await _userManager.FindByEmailAsync("chairman@pts.com") == null)
                {
                    var chairman = new ApplicationUser
                    {
                        UserName = "chairman@pts.com",
                        Email = "chairman@pts.com",
                        FirstName = "adebayo",
                        LastName = "Tijani",
                        ApplicationRoleId = chairmanRole.Id,
                        RoleName = chairmanRole.Name,
                        PhoneNumber = "080767676576",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        DateCreated = DateTime.Now,
                        DateOfBirth = DateTime.Now,
                        Gender = "Male"
                        // add other properties as needed
                    };

                    var result = await _userManager.CreateAsync(chairman, "Chairman12345123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(chairman, "Chairman");
                    }
                    else
                    {
                        // Handle errors if user creation fails
                        Console.WriteLine($"Seeding user failed and these are the errors {result.Errors}");
                    }
                }

                //SEED ADMIN
                if (!await _roleManager.RoleExistsAsync("Administrator"))
                {
                    var adminRole = new ApplicationRole { Name = "Administrator", Description = "In charge of taking inventor" };
                    await _roleManager.CreateAsync(adminRole);

                    if (await _userManager.FindByEmailAsync("admin@pts.com") == null)
                    {
                        var adminUser = new ApplicationUser
                        {
                            UserName = "admin@pts.com",
                            Email = "admin@pts.com",
                            FirstName = "ademola",
                            LastName = "Tijani1",
                            RoleName = adminRole.Name,
                            ApplicationRoleId = adminRole.Id,
                            DateCreated = DateTime.Now,
                            PhoneNumber = "80767623576",
                            PhoneNumberConfirmed = true,
                            EmailConfirmed = true,
                            DateOfBirth = DateTime.Now,
                            Gender = "Male",
                            // add other properties as needed
                        };

                        var result = await _userManager.CreateAsync(adminUser, "Ademola12345123!");

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(adminUser, "Administrator");
                        }
                        else
                        {
                            // Handle errors if user creation fails
                            Console.WriteLine("Seeding user failed");
                        }
                    }
                }


            }
        }

    }
}
