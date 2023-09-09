﻿namespace MoviesAPI.Dtos
{
	public class MovieDto
	{
		public string Title { get; set; }

		public int Year { get; set; }

		public double Rate { get; set; }

		[MaxLength(2500)]
		public string Storeline { get; set; }

		public byte GenreId { get; set; }

	}
}
