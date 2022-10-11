global using dotnetrpg.Models; //Models are available troughout the WebApi
using dotnetrpg.Data;   //Necesary to add the Data Service from Entity Framework
using dotnetrpg.Services.CharacterService; // Necessary to add the Scoped Character Service
using dotnetrpg.Services.WeaponService;
using Microsoft.AspNetCore.Authentication.JwtBearer; //Necessary to add JWT Auth Bearer Scheme
using Microsoft.EntityFrameworkCore; //Necessary to add the Scoped User Authentication Serv.
using Microsoft.IdentityModel.Tokens; // Necessary to add the JWT Bearer token parameters
using Microsoft.OpenApi.Models; // Required for Security Definition in Swagger UI
using Swashbuckle.AspNetCore.Filters; // Implementation of Security Operation Filters

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>
    (options => options.UseSqlServer
        (builder.Configuration // Connection String for Entity Framework Data Context
            .GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( // Enabling the authentication for Swagger UI
    cnfg => 
    {
        cnfg.AddSecurityDefinition("oauth2",new OpenApiSecurityScheme
        {
            Description = "Standard Authorization using JWT Bearer Scheme: [bearer: {token}]",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        cnfg.OperationFilter<SecurityRequirementsOperationFilter>();
        cnfg.SwaggerDoc( "v1", new OpenApiInfo
            {
                Title = "RPG Test API",
                Description = "An API to learn the basics of .NET, JWT and OpenAPI",                
                Contact = new OpenApiContact
                {
                    Name = "Jaciel Israel Reséndiz Ochoa",
                    Email = "jresendizochoa@deloitte.com",
                    Url = new Uri("https://github.com/reoj")
                },
                Version = "v1"
            }    
        );
    }
);

builder.Services.AddHttpContextAccessor(); // Necessary to inyect HTTP context

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>(); // Character Service
builder.Services.AddScoped<IAuthRepository, AuthRepository>(); //Auth. User Service. in Data

// JWT Authentication Scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes
                    (builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        }); 
builder.Services.AddScoped<IWeaponService, WeaponService>(); // Weapon Service
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // JWT Authentication Middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
