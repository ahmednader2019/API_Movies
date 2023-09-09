using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMoviesService _moviesService;
		private readonly IGenreService _genreService;
		private readonly IMapper _mapper;
		private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };

		private long _maxAllowedPosterSize = 1048576;

		public MoviesController(ApplicationDbContext context , IMoviesService moviesService , IGenreService genreService , IMapper mapper)
        {
			_context = context;
			_moviesService = moviesService;
			_genreService = genreService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var movies = _moviesService.GetAll();

			var data = _mapper.Map<IEnumerable<MoviesDetailsDto>>(movies);
			return Ok(data);
		}

		[HttpGet("GenreId")]
		public async Task<IActionResult> GetByGenreID(byte genreid)
		{

			var movies = await _moviesService.GetAll(genreid);


			var data = _mapper.Map<IEnumerable<MoviesDetailsDto>>(movies);
			return Ok(data);

		}

		[HttpGet("{id}")]

		public async Task<IActionResult> GetById(int id)
		{
			var movie = await _moviesService.GetById(id);

			if (movie == null)
				return BadRequest("ID is not Valid");


			var dto = _mapper.Map<MoviesDetailsDto>(movie);

			return Ok(dto);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromForm] MovieCreateDto dto)
		{
			if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
				return BadRequest("only .png and .jpg is valid");

			if (dto.Poster.Length > _maxAllowedPosterSize)
				return BadRequest("Max Allowed Size is 1 MB");

			var isValidGenre = await _genreService.IsValidGenre(dto.GenreId);

			if (!isValidGenre)
				return BadRequest("This Genre ID is not valid");

			using var datastream = new MemoryStream();
			await dto.Poster.CopyToAsync(datastream);

			
			//var movie = new Movie
			//{
			//	GenreId = dto.GenreId,
			//	Year = dto.Year,
			//	Title = dto.Title,
			//	Rate = dto.Rate,
			//	Storeline = dto.Storeline,
			//	Poster = datastream.ToArray()
			//};

			var movie = _mapper.Map<Movie>(dto);
			movie.Poster = datastream.ToArray();

			_moviesService.Add(movie);
			
			return Ok(movie);
		}

		[HttpPut("{id}")]

		public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieUpdateDto dto)
		{
			var movie = await _moviesService.GetById(id);

			if (movie == null)
				return BadRequest("This is is Not Valid");

			// var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

			var isValidGenre = await _genreService.IsValidGenre(dto.GenreId);

			if (!isValidGenre)
				return BadRequest("This Genre ID is not valid");

			if (dto.Poster != null)
			{
				if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
					return BadRequest("only .png and .jpg is valid");

				if (dto.Poster.Length > _maxAllowedPosterSize)
					return BadRequest("Max Allowed Size is 1 MB");

				using var datastream = new MemoryStream();
				await dto.Poster.CopyToAsync(datastream);

				movie.Poster = datastream.ToArray();

			}
			movie.Title = dto.Title;
			movie.Rate = dto.Rate;
			movie.Storeline = dto.Storeline;
			movie.GenreId = dto.GenreId;
			movie.Year = dto.Year;

			_moviesService.Update(movie);

			return Ok(movie);

			
		}


		[HttpDelete("{id}")]

		public async Task<IActionResult> DeleteAsync(int id)
		{
			var movie = await _moviesService.GetById(id);

			if (movie == null)
				return BadRequest("This ID is Not Found");

			_moviesService.Delete(movie);

			return Ok(movie);
		}
	}
}
