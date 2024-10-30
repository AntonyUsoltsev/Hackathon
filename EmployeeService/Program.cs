using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Configuration.AddEnvironmentVariables();


// // Настройка DbContext с использованием строки подключения из переменных окружения
// var connectionString = builder.Configuration.GetConnectionString("EmployeeDb");
// builder.Services.AddDbContext<EmployeeDbContext>(options =>
//     options.UseSqlServer(connectionString));
//
// // Регистрация зависимостей
// builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();
