namespace RagnarApp.Application.DTOs.LibraryDTOs
{
    public class LibraryToReturn
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}
