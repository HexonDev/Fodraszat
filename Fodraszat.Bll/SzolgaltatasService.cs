using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Data;
using Fodraszat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fodraszat.Bll
{
    public class SzolgaltatasService
    {
        private readonly AppDbContext _dbContext;

        public SzolgaltatasService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SzolgaltatasModel>> GetSzolgaltatasokAsync()
        {
            return await _dbContext.Szolgaltatasok.Select(sz => new SzolgaltatasModel
            {
                Idotartam = sz.Idotartam,
                Id = sz.Id,
                Ar = sz.Ar,
                Nev = sz.Nev
            }).ToListAsync();
        }

        public async Task<SzolgaltatasModel> GetSzolgaltatasAsync(int id)
        {
            return await _dbContext.Szolgaltatasok.Select(sz => new SzolgaltatasModel
            {
                Idotartam = sz.Idotartam,
                Id = sz.Id,
                Ar = sz.Ar,
                Nev = sz.Nev
            }).Where(sz => sz.Id == id).SingleOrDefaultAsync();
        }

        public async Task AddSzolgaltatasAsync(SzolgaltatasModel model)
        {
            var szolgaltatas = new Szolgaltatas
            {
                Id = model.Id,
                Ar = model.Ar,
                Idotartam = model.Idotartam,
                Nev = model.Nev
            };

            _dbContext.Szolgaltatasok.Add(szolgaltatas);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditSzolgaltatasAsync(SzolgaltatasModel model)
        {
            var szolgaltatas = await _dbContext.Szolgaltatasok.FindAsync(model.Id);

            szolgaltatas.Id = model.Id;
            szolgaltatas.Ar = model.Ar;
            szolgaltatas.Idotartam = model.Idotartam;
            szolgaltatas.Nev = model.Nev;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSzolgaltatasAsync(SzolgaltatasModel model)
        {
            var szolgaltatas = await _dbContext.Szolgaltatasok.FindAsync(model.Id);

            _dbContext.Szolgaltatasok.Remove(szolgaltatas);
            await _dbContext.SaveChangesAsync();
        }
    }
}