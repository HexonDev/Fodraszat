using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Fodraszat.Data.Seed
{
    public class RoleSeedService : IRoleSeedService
    {
        public RoleManager<IdentityRole<int>> RoleManager { get; }

        public RoleSeedService(RoleManager<IdentityRole<int>> roleManager)
        {
            RoleManager = roleManager;
        }

        public async Task SeedRoleAsync()
        {
            if (!await RoleManager.RoleExistsAsync("Admin"))
                await RoleManager.CreateAsync(new IdentityRole<int> {Name = "Admin"});

            if (!await RoleManager.RoleExistsAsync("Fodrász"))
                await RoleManager.CreateAsync(new IdentityRole<int> {Name = "Fodrász"});
        }
    }
}
