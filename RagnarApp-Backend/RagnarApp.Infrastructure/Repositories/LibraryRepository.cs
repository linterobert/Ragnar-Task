using Microsoft.EntityFrameworkCore;
using RagnarApp.Application.Abstract;
using RagnarApp.Domain.Entities;
using RagnarApp.Infrastructure.Data;

namespace RagnarApp.Infrastructure.Repositories
{
    public class LibraryRepository : GenericRepository<Library>, ILibraryRepository
    {
        public LibraryRepository(RagnarAppContext _context) : base(_context) { }

        public async Task<List<Library>> GetLibrariesOrderByImportDateDesc()
        {
            return await _context.Libraries.OrderByDescending(x => x.ImportDate).ToListAsync();
        }
    }
}
