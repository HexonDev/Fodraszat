using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fodraszat.Data.Seed
{
    public interface IUserSeedService
    {
        Task SeedUserAsync();
    }
}
