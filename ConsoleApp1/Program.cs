using System;
using System.Threading.Tasks;
using Fodraszat.Bll;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.ReadLine();
        }
    }

    public class Telt
    {
        private readonly FoglalasService _foglalasService;

        public Telt(FoglalasService foglalasService)
        {
            _foglalasService = foglalasService;
        }

        public async Task Test()
        {
            var idopontok = await _foglalasService.GetSzabadIdopontokAsync();

            Console.WriteLine(idopontok.Count);

            foreach (var idopont in idopontok)
            {
                Console.WriteLine(idopont.ToString());   
            }
        }
    }
}
