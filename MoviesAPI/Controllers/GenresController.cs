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
	public class GenresController : ControllerBase
	{
		private readonly IGenreService _genreService;

		public GenresController( IGenreService genreService)
        {
			_genreService = genreService;
		}

        [HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var genres = await _genreService.GetAll();

			return Ok(genres);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(GenreDto dto)
		{
			var genre = new Genre { Name = dto.Name };

			await _genreService.Add(genre);
			return Ok(genre);
		}

		[HttpPut("{id}")]

		public async Task<IActionResult> UpdateAsync(byte id ,[FromBody]GenreDto dto)
		{
			var genre = await _genreService.GetById(id);

			if (genre == null)
				return BadRequest($"This ID is Not Found ");

			genre.Name = dto.Name;

			_genreService.Update(genre);

			return Ok(genre);

		}

		[HttpDelete("{id}")]

		public async Task<IActionResult> DeleteAsync(byte id)
		{
			var genre = await _genreService.GetById(id);

			if (genre == null)
				return BadRequest($"This ID is Not Found ");

			_genreService.Delete(genre);

		
			return Ok(genre);
		}





	}
}
