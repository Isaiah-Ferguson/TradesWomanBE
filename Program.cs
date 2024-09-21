using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Services;
using TradesWomanBE.Services.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ClientServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<MeetingsServices>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<ProgramServices>();
builder.Services.AddScoped<CSVServices>();

var connectionString = builder.Configuration.GetConnectionString("TEString");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors(options => {
    options.AddPolicy("TEPolicy", 
    builder => {
        builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("TEPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
