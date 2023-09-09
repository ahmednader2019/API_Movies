using AutoMapper;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Helpers
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Movie, MoviesDetailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
           
        }
    }
}
