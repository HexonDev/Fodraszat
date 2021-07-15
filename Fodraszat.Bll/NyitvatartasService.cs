using Fodraszat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fodraszat.Data.Entities;

namespace Fodraszat.Bll
{
    public class NyitvatartasService
    {
        private readonly AppDbContext _dbContext;

        public NyitvatartasService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<NyitvatartasModel>> GetNyitvatartasAsync(DateTime datumtol, DateTime datumig, int? darab = null)
        {
            var nyitvatartas = _dbContext.Nyitvatartas.Where(ny => ny.Mettol >= datumtol.Date && ny.Mettol < datumig.Date);

            if (darab != null)
                nyitvatartas = nyitvatartas.Take(darab.Value);

            return await nyitvatartas.Select(ny => new NyitvatartasModel
            {
                Id = ny.Id,
                Hossz = ny.Hossz,
                Mettol = ny.Mettol
            }).ToListAsync();
        }

        public async Task<List<NyitvatartasModel>> GetHaviNyitvatartasAsync()
        {
            return await GetNyitvatartasAsync(
                new(DateTime.Today.Year, DateTime.Today.Month, 1),
                new(DateTime.Today.Year, DateTime.Today.Month + 1, 1)
                );
        }

        public async Task<List<NyitvatartasModel>> GetHetiNyitvatartasAsync()
        {
            var hetfo = DateTime.Today;
            while (hetfo.DayOfWeek != DayOfWeek.Monday)
                hetfo = hetfo.AddDays(-1);

            return await GetNyitvatartasAsync(hetfo, hetfo.AddDays(7));
        }

        public async Task<NyitvatartasModel> GetNyitvatartasByIdAsync(int id)
        {
            return await _dbContext.Nyitvatartas.Select(ny => new NyitvatartasModel
            {
                Id = ny.Id,
                Hossz = ny.Hossz,
                Mettol = ny.Mettol
            }).Where(ny => ny.Id == id).SingleOrDefaultAsync();
        }

        public async Task AddNyitvatartasAsync(NyitvatartasModel model)
        {
            var nyitvatartas = new Nyitvatartas
            {
                Hossz = model.Hossz,
                Mettol = model.Mettol,
                Megjegyzes = model.Megjegyzes
            };

            _dbContext.Nyitvatartas.Add(nyitvatartas);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditNyitvatartasAsync(NyitvatartasModel model)
        {
            var nyitvatartas = await _dbContext.Nyitvatartas.FindAsync(model.Id);

            nyitvatartas.Hossz = model.Hossz;
            nyitvatartas.Megjegyzes = model.Megjegyzes;
            nyitvatartas.Mettol = model.Mettol;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteNyitvatartasAsync(NyitvatartasModel model)
        {
            var nyitvatartas = await _dbContext.Nyitvatartas.FindAsync(model.Id);
            _dbContext.Nyitvatartas.Remove(nyitvatartas);
            await _dbContext.SaveChangesAsync();
        }
    }
}