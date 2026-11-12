using System.ComponentModel.DataAnnotations;

namespace GrebennikovaM_LR5.Models;

// Завдання 1. DTO для захисту від over-posting (поле Secret не передається клієнту)
public class TodoItemDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    // Завдання 2. Дата виконання - обов'язкова при створенні та оновленні
    [Required(ErrorMessage = "Поле DueDate (дата виконання) є обов'язковим.")]
    public DateOnly? DueDate { get; set; }

    // Завдання 2. Пріоритет - ціле число від 1 до 5
    [Range(1, 5, ErrorMessage = "Поле Priority (пріоритет) має бути в межах від 1 до 5.")]
    public int Priority { get; set; }
}
