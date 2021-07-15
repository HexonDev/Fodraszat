using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fodraszat.Data.Seed
{
    public class UserSeedService : IUserSeedService
    {
        public UserManager<Felhasznalo> UserManager { get; }

        public UserSeedService(UserManager<Felhasznalo> userManager)
        {
            UserManager = userManager;
        }

        public async Task SeedUserAsync()
        {
            if (!(await UserManager.GetUsersInRoleAsync("Admin")).Any())
            {
                var felhasznalo = new Felhasznalo
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Nev = "Alapértelmezett Adminisztrátor",
                    EmailConfirmed = true,
                    PhoneNumber = "12345678910",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var createResult = await UserManager.CreateAsync(felhasznalo, "Admin123!");

                if (UserManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(felhasznalo);
                    var result = await UserManager.ConfirmEmailAsync(felhasznalo, code);
                }

                var roleResult = await UserManager.AddToRoleAsync(felhasznalo, "Admin");

                if (!createResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült létrehozni az admin felhasználót: "
                                                   + string.Join(", ", createResult.Errors.Concat(roleResult.Errors).Select(e => e.Description)));
                }
            }

        }
    }
}
