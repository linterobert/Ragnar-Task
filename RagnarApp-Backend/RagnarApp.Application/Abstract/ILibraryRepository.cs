using RagnarApp.Domain.Entities;

namespace RagnarApp.Application.Abstract
{
    public interface ILibraryRepository : IGenericRepository<Library>
    {
        Task<List<Library>> GetLibrariesOrderByImportDateDesc();
    }
}
