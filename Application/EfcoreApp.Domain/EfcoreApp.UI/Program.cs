using System;
using System.Linq;
using EfcoreApp.Domain;
using EfcoreApp.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EfcoreApp.UI
{
    class Program
    {
        private static EfcoreContext _context = new EfcoreContext();

        private static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            GetSamurais("Before Add:");
            AddSamurai();
            GetSamurais("After Add:");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Julie" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith("Getting all the samurais saved on the database")
                .ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
