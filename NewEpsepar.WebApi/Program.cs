

using Microsoft.EntityFrameworkCore;
using NewEpsepar.Infrastructure;
using NewEpsepar.Infrastructure.Repositories;
using NewEpsepar.Domain.Interfaces;
using NewEpsepar.Application.Usuarios;
using NewEpsepar.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de EF Core y DbContext
builder.Services.AddDbContext<EpseparDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Registrar repositorio y servicio de usuario
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Nuevo endpoint: crear usuario
app.MapPost("/usuarios", async (CrearUsuarioCommand cmd, UsuarioService usuarioService) =>
{
    var usuario = new NewEpsepar.Domain.Usuario
    {
        Nombre = cmd.Nombre,
        Email = cmd.Email,
        Activo = true
    };
    await usuarioService.CrearUsuarioAsync(usuario);
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
})
.WithName("CrearUsuario");

// Nuevo endpoint: listar usuarios
app.MapGet("/usuarios", async (IUsuarioRepository repo) =>
{
    var usuarios = await repo.GetAllAsync();
    return Results.Ok(usuarios);
})
.WithName("ListarUsuarios");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
