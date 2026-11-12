namespace GrebennikovaM_LR5.Models;

// Завдання 1. Модель TodoItem згідно з інструкцією Microsoft Learn
public class TodoItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }

    // Завдання 2. Нове поле: дата виконання завдання
    public DateOnly DueDate { get; set; }

    // Завдання 2. Нове поле: пріоритет завдання (1 - найвищий, 5 - найнижчий)
    public int Priority { get; set; }
}
