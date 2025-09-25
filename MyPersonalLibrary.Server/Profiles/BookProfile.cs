using AutoMapper;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.DTOs;

namespace MyPersonalLibrary.Server.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
