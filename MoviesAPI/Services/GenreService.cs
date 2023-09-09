using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;
using System.Collections;

namespace MoviesAPI.Services
{
	public class GenreService : IGenreService
	{
		private readonly ApplicationDbContext _context;

		public GenreService(ApplicationDbContext context)
        {
			_context = context;
		}

		public async Task<IEnumerable> GetAll()
		{
			var genres = await _context.Genres.OrderBy(O => O.Name).ToListAsync();

			return genres;
		}

		public async Task<Genre> GetById(byte id)
		{
			var genre = await _context.Genres.SingleOrDefaultAsync(I => I.Id == id);

			return genre; 
		}
		public async Task<Genre> Add(Genre genre)
		{
			await _context.Genres.AddAsync(genre);

			_context.SaveChanges();

			return genre;
		}

		public Genre Update(Genre genre)
		{
		    _context.Genres.Update(genre);

			_context.SaveChanges();

			return genre;
		}

		public Genre Delete(Genre genre)
		{
			_context.Genres.Remove(genre);

			_context.SaveChanges();

			return genre;
		}

		public async Task<bool> IsValidGenre(byte id)
		{
			return await _context.Genres.AnyAsync(g => g.Id == id);
		}
	}
}
