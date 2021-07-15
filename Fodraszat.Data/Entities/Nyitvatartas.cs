using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fodraszat.Data.Entities
{
    public class Nyitvatartas : EntityBase<Nyitvatartas>
    {
        public DateTime Mettol { get; set; }
        public uint Hossz { get; set; }
    }
}
