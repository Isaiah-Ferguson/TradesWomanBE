using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TradesWomanBE.Services;
using TradesWomanBE.Services.Context;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ClientServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<MeetingsServices>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<ProgramServices>();
builder.Services.AddScoped<CSVServices>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Database connection
var connectionString = builder.Configuration.GetConnectionString("TEString");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// CORS configuration
builder.Services.AddCors(options => {
    options.AddPolicy("TEPolicy", 
    builder => {
        builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://thankful-coast-03112031e.5.azurestaticapps.net/")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// JWT Authentication Configuration
var secretKey = builder.Configuration["Jwt:Key"] ?? "superSecretKey@345"; // Fallback to hardcoded secret if not in configuration

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
        ValidIssuers = new[] 
        { 
            "https://thankful-coast-03112031e.5.azurestaticapps.net", // Hosted site
            "http://localhost:5000"  // Local testing
        },
        ValidAudiences = new[] 
        { 
            "https://thankful-coast-03112031e.5.azurestaticapps.net", // Hosted site
            "http://localhost:5000"  // Local testing
        },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Secret key
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

// Middleware
app.UseCors("TEPolicy");
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
