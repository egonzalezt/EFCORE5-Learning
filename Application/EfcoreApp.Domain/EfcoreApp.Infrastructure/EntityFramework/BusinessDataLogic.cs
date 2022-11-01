using Microsoft.EntityFrameworkCore;
using EfcoreApp.Domain;
using System.Linq;


namespace EfcoreApp.Infrastructure.EntityFramework
{
    public class BusinessDataLogic
    {
        private EfcoreContext _context;

        public BusinessDataLogic(EfcoreContext context)
        {
            _context = context;
        }

        public BusinessDataLogic()
        {
            _context = new EfcoreContext();
        }

        public int AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            var dbResult = _context.SaveChanges();
            return dbResult;
        }

        public int InsertNewSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            var dbResult = _context.SaveChanges();
            return dbResult;
        }

        public Samurai GetSamuraiWithQuotes(int samuraiId)
        {
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Id == samuraiId)
                                                     .Include(s => s.Quotes)
                                                     .FirstOrDefault();
            return samuraiWithQuotes;
        }
    }
}
