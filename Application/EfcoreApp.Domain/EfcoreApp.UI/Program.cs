using EfcoreApp.Infrastructure.EntityFramework;

namespace EfcoreApp.UI
{
    class Program
    {
        private static void Main(string[] args)
        {
            AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
        }

        private static void AddSamuraisByName(params string[] names)
        {
            var _bizData = new BusinessDataLogic();
            var newSamuraisCreatedCount = _bizData.AddSamuraisByName(names);
        }
    }
}
