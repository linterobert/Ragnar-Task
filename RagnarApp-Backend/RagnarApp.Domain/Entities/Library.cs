using RagnarApp.Domain.Enums;

namespace RagnarApp.Domain.Entities
{
    public class Library
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public GenreEnum Genre { get; set; }
        public DateTimeOffset ImportDate { get; set; }
    }
}
