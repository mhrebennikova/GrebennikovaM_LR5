using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GrebennikovaM_LR5.Models;
using GrebennikovaM_LR5.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Завдання 3. Прив'язка секції "Database" з конфігурації та валідація
// за допомогою System.ComponentModel.DataAnnotations під час запуску застосунку
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Завдання 1, 3. Реєстрація контексту БД; ім'я БД береться з конфігурації замість "TodoList"
builder.Services.AddDbContext<TodoContext>((serviceProvider, opt) =>
{
    var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    opt.UseInMemoryDatabase(databaseOptions.Name!);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

// Завдання 3. Виведення помилок валідації конфігурації, якщо ім'я БД некоректне
try
{
    app.Run();
}
catch (OptionsValidationException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Помилка конфігурації бази даних:");
    foreach (var failure in ex.Failures)
    {
        Console.WriteLine($" - {failure}");
    }
    Console.ResetColor();
    Environment.ExitCode = 1;
}
