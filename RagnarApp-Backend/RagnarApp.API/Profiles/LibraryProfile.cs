using AutoMapper;
using RagnarApp.Application.DTOs.LibraryDTOs;
using RagnarApp.Domain.Entities;

namespace RagnarApp.API.Profiles
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<Library, LibraryToReturn>()
                .ForMember(
                    dest => dest.Genre,
                    opt => opt.MapFrom(src => src.Genre.ToString()));
        }
    }
}
