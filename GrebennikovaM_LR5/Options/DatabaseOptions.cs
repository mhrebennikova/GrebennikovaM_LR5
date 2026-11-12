using System.ComponentModel.DataAnnotations;

namespace GrebennikovaM_LR5.Options;

// Завдання 3. Налаштування бази даних, які зчитуються з конфігурації (appsettings.json, секція "Database")
public class DatabaseOptions
{
    public const string SectionName = "Database";

    // Завдання 3. Валідація імені БД: не null, не порожнє, не менше 4 символів
    [Required(ErrorMessage = "Ім'я бази даних (Database:Name) не задане або порожнє.")]
    [MinLength(4, ErrorMessage = "Ім'я бази даних (Database:Name) має містити щонайменше 4 символи.")]
    public string? Name { get; set; }
}
