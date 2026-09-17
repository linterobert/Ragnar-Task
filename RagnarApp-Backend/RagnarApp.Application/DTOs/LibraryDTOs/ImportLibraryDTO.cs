namespace RagnarApp.Application.DTOs.LibraryDTOs
{
    using CsvHelper.Configuration.Attributes;

    public class ImportLibraryDTO
    {
        [Name("name")]
        public string Name { get; set; } = string.Empty;

        [Name("author")]
        public string Author { get; set; } = string.Empty;

        [Name("genre")]
        public string Genre { get; set; } = string.Empty;
    }
}
