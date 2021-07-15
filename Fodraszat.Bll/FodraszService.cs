using Fodraszat.Data;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fodraszat.Bll
{
    public class FodraszService
    {
        private readonly UserManager<Felhasznalo> _userManager;

        public FodraszService(UserManager<Felhasznalo> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<FodraszModel>> GetFodraszokAsync()
        {
            return (await _userManager.GetUsersInRoleAsync("Fodrász")).Select(f => new FodraszModel
            {
                Id = f.Id,
                Nev = f.Nev
            }).ToList();
        }

        public async Task<FodraszModel?> GetFodraszAsync(int id)
        {
            return (await _userManager.GetUsersInRoleAsync("Fodrász")).Select(f => new FodraszModel
            {
                Id = f.Id,
                Nev = f.Nev
            }).SingleOrDefault(f => f.Id == id);
        }
    }
}
