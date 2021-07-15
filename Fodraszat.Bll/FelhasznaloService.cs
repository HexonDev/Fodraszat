using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fodraszat.Data;
using Microsoft.EntityFrameworkCore;

namespace Fodraszat.Bll
{
    public class FelhasznaloService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<Felhasznalo> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;


        public FelhasznaloService(AppDbContext dbContext, UserManager<Felhasznalo> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<FelhasznaloModel>> GetFelhasznalokAsync()
        {
            return await _dbContext.Users.Select(f => new FelhasznaloModel
            {
                Id = f.Id,
                Email = f.Email,
                Nev = f.Nev,
                SzuletesiIdo = f.SzuletesiIdo,
                Telefonszam = f.PhoneNumber
            }).ToListAsync();
        }

        public async Task<FelhasznaloModel> GetFelhasznaloAsync(int id)
        {
            return await _dbContext.Users.Select(f => new FelhasznaloModel
            {
                Id = f.Id,
                Email = f.Email,
                Nev = f.Nev,
                SzuletesiIdo = f.SzuletesiIdo,
                Telefonszam = f.PhoneNumber
            }).Where(f => f.Id == id)
            .SingleOrDefaultAsync();
        }

        public async Task<IdentityResult> AddFelhasznaloAsync(RegisztracioModel model)
        {
            var felhasznalo = new Felhasznalo
            {
                UserName = model.Email,
                Email = model.Email,
                Nev = model.FullName,
                PhoneNumber = model.PhoneNumber,
                SzuletesiIdo = model.DateOfBirth,
                EmailConfirmed = true
            };

            return await _userManager.CreateAsync(felhasznalo, model.Password);
        }

        public async Task DeleteFelhasznaloAsync(FelhasznaloModel model)
        {
            var felhasznalo = await _dbContext.Users.FindAsync(model.Id);
            await _userManager.DeleteAsync(felhasznalo);
        }

        public async Task<IdentityResult> EditFelhasznaloAsync(FelhasznaloModel model)
        {
            var felhasznalo = await _dbContext.Users.FindAsync(model.Id);

            felhasznalo.Email = model.Email;
            felhasznalo.UserName = model.Email;
            felhasznalo.Nev = model.Nev;
            felhasznalo.SzuletesiIdo = model.SzuletesiIdo;
            felhasznalo.PhoneNumber = model.Telefonszam;

            return await _userManager.UpdateAsync(felhasznalo);
        }

        public async Task<IdentityResult> AddFelhasznaloToRoleAsync(FelhasznaloModel model, string role)
        {
            var felhasznalo = await _dbContext.Users.FindAsync(model.Id);

            return await _userManager.AddToRoleAsync(felhasznalo, role);
        }

        public async Task<IdentityResult> RemoveFelhasznaloFromRoleAsync(FelhasznaloModel model, string role)
        {
            var felhasznalo = await _dbContext.Users.FindAsync(model.Id);
            return await _userManager.RemoveFromRoleAsync(felhasznalo, role);
        }

        public async Task<string[]> GetFelhasznaloRolesAsync(FelhasznaloModel model)
        {
            var felhasznalo = await _dbContext.Users.FindAsync(model.Id);
            return (await _userManager.GetRolesAsync(felhasznalo)).ToArray();
        }
    }
}
