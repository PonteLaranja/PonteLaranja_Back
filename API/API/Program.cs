using API.Applications.Services;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using API.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// CONEXÃO COM O BANCO
builder.Services.AddDbContext<PonteLaranjaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// UNIDADE
builder.Services.AddScoped<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddScoped<UnidadeService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
