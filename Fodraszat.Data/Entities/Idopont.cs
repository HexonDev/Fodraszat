using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fodraszat.Data.Entities
{
    public class Idopont : EntityBase<Idopont>
    {
        public DateTime Datum { get; set; }
        public Szolgaltatas Szolgaltatas { get; set; }
        public int SzolgaltatasId { get; set; }
        public Felhasznalo Felhasznalo { get; set; }
        public int FelhasznaloId { get; set; }
        public Felhasznalo Fodrasz { get; set; }
        public int FodraszId { get; set; }

        public override void Configure(EntityTypeBuilder<Idopont> builder)
        {
            builder.HasIndex(i => new {i.Datum, i.FelhasznaloId, i.FodraszId, i.SzolgaltatasId}).IsUnique(true);
        }
    }
}
