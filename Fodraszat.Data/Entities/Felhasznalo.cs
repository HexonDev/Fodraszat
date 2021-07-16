using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fodraszat.Data.Entities
{
    public class Felhasznalo : IdentityUser<int>
    {
        public string Nev { get; set; }
        public DateTime SzuletesiIdo { get; set; }
        public string? Leiras { get; set; }
    }
}
