using Fodraszat.Data;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fodraszat.Bll
{
    public class FoglalasService
    {
        private readonly AppDbContext _dbContext;
        private readonly SzolgaltatasService _szolgaltatasService;
        private readonly NyitvatartasService _nyitvatartasService;


        public FoglalasService(AppDbContext dbContext, SzolgaltatasService szolgaltatasService, NyitvatartasService nyitvatartasService)
        {
            _dbContext = dbContext;
            _szolgaltatasService = szolgaltatasService;
            _nyitvatartasService = nyitvatartasService;
        }

        public async Task<List<DateTime>> GetSzabadIdopontokAsync(int fodraszId, int szolgalatasId, DateTime datumtol, DateTime datumig)
        {
            var legkisebbIdotartam = (await _szolgaltatasService.GetSzolgaltatasokAsync()).Select(sz => sz.Idotartam).Min();
            var kivalasztottSzolgaltatas = await _szolgaltatasService.GetSzolgaltatasAsync(szolgalatasId);

            var nyitvatartas = (await _nyitvatartasService.GetNyitvatartasAsync(datumtol, datumig))
                .OrderBy(ny => ny.Mettol).ToList();

            var idopontok = new List<DateTime>();
            var lefoglaltIdopontok = _dbContext.Idopontok.Where(i => i.FodraszId == fodraszId)
                .Select(i => new IdopontModel
                {
                    Id = i.Id,
                    FelhasznaloId = i.FelhasznaloId,
                    FodraszId = i.FodraszId,
                    SzolgaltatasId = i.SzolgaltatasId,
                    Datum = i.Datum,
                    SzolgaltatasHossz = i.Szolgaltatas.Idotartam
                }).ToList();

            foreach (var ny in nyitvatartas) // Végig nézzk a nyitvatartási napokat, ami alapján...
            {
                for (DateTime idopont = ny.Mettol; // .. időpontokat generálunk a legrövidebbig tartó szolgáltatás ideje alapján...
                    idopont <= ny.Meddig;
                    idopont = idopont.AddMinutes(legkisebbIdotartam))
                {
                    if (idopont > DateTime.Now) // .. ha a generált időpont nagyobb, mint a mostani dátum, akkor ...
                    {
                        idopontok.Add(idopont); // .. belerakjuk a listába.
                        
                        if (lefoglaltIdopontok.Count > 0) // Ha van lefoglalt időpont, akkor...
                        {
                            foreach (var lefoglaltIdopont in lefoglaltIdopontok) // .. végig megyünk a lefoglalt időpontokon...
                            {
                                if (idopont >= lefoglaltIdopont.Datum.AddMinutes(-(kivalasztottSzolgaltatas.Idotartam - 1)) && idopont <= lefoglaltIdopont.Datum.AddMinutes(lefoglaltIdopont.SzolgaltatasHossz - 1)) // ...ha a generált időpont a (lefoglalt időpont - kiválasztott szolgáltatás időtartama) + (lefoglalt időpont + szolgáltatás hossz) közé esik, akkor...
                                    idopontok.Remove(idopont); // .. eltávolítjuk a generált időpontot a szabad időpontok listából.

                                if (idopont.AddMinutes(kivalasztottSzolgaltatas.Idotartam - 1) > ny.Meddig) // Ha a (generált időpont + kiválasztott szolgáltatás időtartama) a nyitvatartáson kívűlre nyúlik (szóval nagyobb, mint a nyitvatartási iidő), akkor...
                                    idopontok.Remove(idopont); // .. eltávolítjuk a generált időpontot a szabad időpontok listából.
                            }
                        }
                    }
                }
            }

            return idopontok;
        }

        public async Task<bool> AddIdopontAsync(IdopontModel model)
        {

            var kivalasztottSzolgaltatas = await _szolgaltatasService.GetSzolgaltatasAsync(model.SzolgaltatasId);
            var lefoglaltIdopontok = _dbContext.Idopontok.Where(i => i.FodraszId == model.FodraszId)
                .Select(i => new IdopontModel
                {
                    Id = i.Id,
                    FelhasznaloId = i.FelhasznaloId,
                    FodraszId = i.FodraszId,
                    SzolgaltatasId = i.SzolgaltatasId,
                    Datum = i.Datum,
                    SzolgaltatasHossz = i.Szolgaltatas.Idotartam
                }).ToList();

            // ellenőrizzk, hogy nem foglalt-e az időpont
            if (lefoglaltIdopontok.Count > 0) // Ha van lefoglalt időpont, akkor...
            {
                foreach (var lefoglaltIdopont in lefoglaltIdopontok) // .. végig megyünk a lefoglalt időpontokon...
                {
                    if (model.Datum >= lefoglaltIdopont.Datum.AddMinutes(-(kivalasztottSzolgaltatas.Idotartam - 1)) && model.Datum <= lefoglaltIdopont.Datum.AddMinutes(lefoglaltIdopont.SzolgaltatasHossz - 1)) // ...ha a generált időpont a (lefoglalt időpont - kiválasztott szolgáltatás időtartama) + (lefoglalt időpont + szolgáltatás hossz) közé esik, akkor...
                        return false; // .. visszaadjuk a sikeretlen foglalást.
                }
            }
            
            Idopont idopont = new()
            {
                Datum = model.Datum,
                SzolgaltatasId = model.SzolgaltatasId,
                FelhasznaloId = model.FelhasznaloId,
                FodraszId = model.FodraszId
            };

            _dbContext.Idopontok.Add(idopont);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<IdopontModel>> GetIdopontokAsync(int? fodraszId = null, int? felhasznaloId = null)
        {
            var idopontok = await _dbContext.Idopontok
            .Include(i => i.Felhasznalo)
            .Include(i => i.Fodrasz)
            .Include(i => i.Szolgaltatas)
            .Select(i => new IdopontModel
            {
                Id = i.Id,
                FelhasznaloId = i.FelhasznaloId,
                FelhasznaloNev = i.Felhasznalo.Nev,
                FodraszId = i.FodraszId,
                FodraszNev = i.Fodrasz.Nev,
                SzolgaltatasId = i.SzolgaltatasId,
                SzolgaltatasNev = i.Szolgaltatas.Nev,
                SzolgaltatasHossz = i.Szolgaltatas.Idotartam,
                Datum = i.Datum
            }).ToListAsync();

            if (fodraszId != null)
                idopontok = idopontok.Where(i => i.FodraszId == fodraszId.Value).ToList();

            if(felhasznaloId != null)
                idopontok = idopontok.Where(i => i.FelhasznaloId == felhasznaloId.Value).ToList();

            return idopontok;
        }
    }
}
