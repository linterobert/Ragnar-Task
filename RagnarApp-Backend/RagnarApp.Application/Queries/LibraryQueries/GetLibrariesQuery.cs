using MediatR;
using RagnarApp.Domain.Entities;

namespace RagnarApp.Application.Queries.LibraryQueries
{
    public class GetLibrariesQuery : IRequest<List<Library>>
    { 
    }
}
