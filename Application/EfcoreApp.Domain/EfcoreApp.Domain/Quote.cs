namespace EfcoreApp.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Samurai samurai { get; set; }
        public int SamuraiId { get; set; }
    }
}
