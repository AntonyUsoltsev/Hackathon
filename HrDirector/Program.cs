using HrDirector.DataBase;
using HrDirector.Repository;
using HrDirector.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHrDirectorService, HrDirectorService>();
builder.Services.AddScoped<SatisfactionRepository>();
builder.Services.AddScoped<HackathonRepository>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<WishlistRepository>();


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HackathonContext>(options => options.UseNpgsql(connectionString));

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
app.UseRouting();

app.MapControllers();

app.Run();