using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TradesWomanBE.Services;
using TradesWomanBE.Services.Context;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// This is a test
// Add services to the container.
builder.Services.AddScoped<ClientServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<MeetingsServices>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<ProgramServices>();
builder.Services.AddScoped<CSVServices>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// CORS configuration
builder.Services.AddCors(options => {
    options.AddPolicy("TWPolicy", 
    builder => {
        builder.WithOrigins("http://localhost:3000", "https://tradeswomen-client.vercel.app")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = jwtSettings.GetSection("ValidIssuers").Get<string[]>(), 
        ValidAudiences = jwtSettings.GetSection("ValidAudiences").Get<string[]>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("TWPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
