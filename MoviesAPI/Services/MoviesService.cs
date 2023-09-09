using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
	public class MoviesService : IMoviesService
	{
		private readonly ApplicationDbContext _context;

		public MoviesService(ApplicationDbContext context)
        {
			_context = context;
		}
        public async Task<Movie> Add(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
			_context.SaveChanges();

			return movie;
		}

		public  Movie Delete(Movie movie)
		{
			 _context.Movies.Remove(movie);
			_context.SaveChanges();

			return movie;
		}

		public async Task<IEnumerable<Movie>>GetAll(byte genreId = 0)
		{
			var movies = _context.Movies
				            .Where(m => m.GenreId == genreId || genreId == 0) 
							.OrderByDescending(m => m.Rate)
							.Include(m => m.Genre)
							.ToList();
			return movies;
		}

		public async Task<Movie> GetById(int id)
		{
		   return  await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

		}

		public Movie Update(Movie movie)
		{
			_context.Movies.Update(movie);
			_context.SaveChanges();

			return movie;
		}
	}
}
