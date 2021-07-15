using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Fodraszat.Bll
{
    public class FelhasznaloModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Teljes név")]
        public string Nev { get; set; }

        [Required]
        [Phone]
        [PersonalData]
        [Display(Name = "Telefonszám")]
        public string Telefonszam { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "Születési idő")]
        public DateTime SzuletesiIdo { get; set; }
    }

    public class RegisztracioModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítése")]
        [Compare("Password", ErrorMessage = "A beírt jelszavak nem egyeznek.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "Születési idő")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "Teljes név")]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefonszám")]
        public string PhoneNumber { get; set; }
    }

    public class NyitvatartasModel
    {
        public int Id { get; set; }

        [Display(Name = "Mettől")]
        [Required]
        public DateTime Mettol { get; set; }
        
        [Display(Name = "Hossz (perc)")]
        [Required]
        public uint Hossz { get; set; }

        public DateTime Meddig => Mettol.AddMinutes(Hossz);
        public string? Megjegyzes { get; set; }
    }

    public class SzolgaltatasModel
    {
        public int Id { get; set; }

        [Display(Name = "Név")]
        public string Nev { get; set; }

        [Display(Name = "Ár")]
        public decimal Ar { get; set; }

        [Display(Name = "Időtartam (perc)")]
        public uint Idotartam { get; set; }

        [Display(Name = "Megjegyzés")]
        public string? Megjegyzes { get; set; }
    }

    public class FodraszModel
    {
        public int Id { get; set; }
        public string Nev { get; set; }
    }

    public class IdopontModel
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public string FelhasznaloNev { get; set; }
        public int FodraszId { get; set; }
        public string FodraszNev { get; set; }
        public int SzolgaltatasId { get; set; }
        public string SzolgaltatasNev { get; set; }
        public uint SzolgaltatasHossz { get; set; }
        public DateTime Datum { get; set; }
    }
}
