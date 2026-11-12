using Microsoft.EntityFrameworkCore;

namespace GrebennikovaM_LR5.Models;

// Завдання 1. Контекст бази даних згідно з інструкцією Microsoft Learn
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}
