using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fodraszat.Data.Entities
{
    public class Szolgaltatas : EntityBase<Szolgaltatas>
    {
        public string Nev { get; set; }
        public decimal Ar { get; set; }
        public uint Idotartam { get; set; }

    }
}
