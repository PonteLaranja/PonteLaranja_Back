using API.Applications.Services;
using API.Contexts;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<PonteLaranjaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<TipoUsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<ITipoMedidaRepository, TipoMedidaRepository>();
builder.Services.AddScoped<TipoMedidaService>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//        });
//});


app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
