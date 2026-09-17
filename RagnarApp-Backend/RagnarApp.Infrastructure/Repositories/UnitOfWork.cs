using RagnarApp.Application.Abstract;
using RagnarApp.Infrastructure.Data;

namespace RagnarApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RagnarAppContext _context;

        public UnitOfWork(RagnarAppContext context, ILibraryRepository libraryRepository)
        {
            _context = context;
            LibraryRepository = libraryRepository;
        }
        public ILibraryRepository LibraryRepository { get; private set; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
