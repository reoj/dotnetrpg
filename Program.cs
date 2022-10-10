global using dotnetrpg.Models; //Models are available troughout the WebApi
using dotnetrpg.Data;   //Necesary to add the Data Service from Entity Framework
using dotnetrpg.Services.CharacterService; // Necessary to add the Scoped Character Service
using Microsoft.EntityFrameworkCore; //Necessary to add the Scoped User Authentication Serv.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // connection String for Entity Framework Data Context
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>(); // Character Service
builder.Services.AddScoped<IAuthRepository, AuthRepository>(); //Auth. User Service. in Data

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
