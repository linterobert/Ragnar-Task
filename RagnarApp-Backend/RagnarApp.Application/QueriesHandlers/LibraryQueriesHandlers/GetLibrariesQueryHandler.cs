using MediatR;
using RagnarApp.Application.Abstract;
using RagnarApp.Application.Queries.LibraryQueries;
using RagnarApp.Domain.Entities;

namespace RagnarApp.Application.QueriesHandlers.LibraryQueriesHandlers
{
    internal class GetLibrariesQueryHandler : IRequestHandler<GetLibrariesQuery, List<Library>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetLibrariesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Library>> Handle(GetLibrariesQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.LibraryRepository.GetLibrariesOrderByImportDateDesc();
        }
    }
}
