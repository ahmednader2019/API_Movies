using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesAPI.Models;
using MoviesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(ConnectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddTransient<IGenreService,GenreService>();
builder.Services.AddTransient<IMoviesService, MoviesService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo 
	{
	 Version = "v1",	
	 Title = "MoviesAPI"
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
