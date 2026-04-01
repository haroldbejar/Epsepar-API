


using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NewEpsepar.Infrastructure;
using NewEpsepar.Infrastructure.Repositories;
using NewEpsepar.Domain.Interfaces;
using NewEpsepar.Application.Usuarios;
using NewEpsepar.Application;
using NewEpsepar.Application.Empleados;
using NewEpsepar.Application.Clientes;
using NewEpsepar.Application.Beneficiarios;
using NewEpsepar.Application.Planillas;
using NewEpsepar.Application.Sedes;
using NewEpsepar.Application.EmpresasPrestadoras;
using NewEpsepar.Application.ARLs;

var builder = WebApplication.CreateBuilder(args);

// Configuración de IdentityDbContext para autenticación
builder.Services.AddDbContext<EpseparIdentityDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<EpseparIdentityDbContext>()
    .AddDefaultTokenProviders();

// Configuración de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? ""))
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(NewEpsepar.WebApi.MappingProfile));

// Configuración de EF Core y DbContext
builder.Services.AddDbContext<EpseparDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Registrar repositorio y servicio de usuario

// Repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
builder.Services.AddScoped<IPlanillaRepository, PlanillaRepository>();
builder.Services.AddScoped<ISedeRepository, SedeRepository>();
builder.Services.AddScoped<IEmpresaPrestadoraRepository, EmpresaPrestadoraRepository>();
builder.Services.AddScoped<IARLRepository, ARLRepository>();

// Servicios de aplicación
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<BeneficiarioService>();
builder.Services.AddScoped<PlanillaService>();
builder.Services.AddScoped<SedeService>();
builder.Services.AddScoped<EmpresaPrestadoraService>();
builder.Services.AddScoped<ARLService>();


// Registrar servicios de autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

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


// Mapeo de controladores MVC
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
