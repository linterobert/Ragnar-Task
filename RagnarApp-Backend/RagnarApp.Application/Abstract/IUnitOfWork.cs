namespace RagnarApp.Application.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        public ILibraryRepository LibraryRepository { get; }
        Task Save();
    }
}
