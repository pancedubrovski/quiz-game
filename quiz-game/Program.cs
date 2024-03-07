using Microsoft.EntityFrameworkCore;
using quiz_game.Database;
using quiz_game.Hubs;
using quiz_game.Repositories;
using quiz_game.Repositories.Interfaces;
using quiz_game.Services;
using quiz_game.Services.interfalces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<QuizGameContext>(options =>
                options.UseSqlServer("Server=localhost;Database=QuizGame;User Id=sa;Password=Bidat123;TrustServerCertificate=True"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserRepository, UserRepositroy>();


// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<GameHub>("/hubs/game");

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
