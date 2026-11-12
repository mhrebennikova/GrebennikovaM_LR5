using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrebennikovaM_LR5.Models;

namespace GrebennikovaM_LR5.Controllers;

// Завдання 1. Контролер, згенерований за інструкцією Microsoft Learn (з використанням DTO)
[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // Завдання 1, 2. Отримання всіх записів (з новими полями DueDate і Priority)
    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // Завдання 4. Пошук записів за двома та більше Id
    // GET: api/TodoItems/by-ids?ids=1&ids=2&ids=3
    [HttpGet("by-ids")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems([FromQuery] long[] ids)
    {
        var distinctIds = ids.Distinct().ToArray();

        if (distinctIds.Length < 2)
        {
            return BadRequest("Для пошуку потрібно передати щонайменше два різні Id (наприклад: ?ids=1&ids=2).");
        }

        var todoItems = await _context.TodoItems
            .Where(x => distinctIds.Contains(x.Id))
            .OrderBy(x => x.Id)
            .Select(x => ItemToDTO(x))
            .ToListAsync();

        if (todoItems.Count == 0)
        {
            return NotFound($"Записів з Id {string.Join(", ", distinctIds)} не знайдено.");
        }

        return todoItems;
    }

    // Завдання 1, 2. Отримання одного запису за Id
    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }

    // Завдання 1, 2. Оновлення запису (оновлюються також DueDate і Priority)
    // PUT: api/TodoItems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;
        // Завдання 2. Оновлення нових полів
        todoItem.DueDate = todoDTO.DueDate!.Value;
        todoItem.Priority = todoDTO.Priority;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // Завдання 1, 2. Створення одного запису (з полями DueDate і Priority)
    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = DTOToItem(todoDTO);

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    // Завдання 5. Масове створення записів: приймає масив об'єктів
    // і повертає всі записані items разом з присвоєними Id
    // POST: api/TodoItems/batch
    [HttpPost("batch")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> PostTodoItems(List<TodoItemDTO> todoDTOs)
    {
        if (todoDTOs == null || todoDTOs.Count == 0)
        {
            return BadRequest("Масив записів для створення порожній.");
        }

        var todoItems = todoDTOs.Select(DTOToItem).ToList();

        _context.TodoItems.AddRange(todoItems);
        await _context.SaveChangesAsync();

        var createdItems = todoItems.Select(ItemToDTO).ToList();

        // Заголовок Location вказує на метод пошуку за кількома Id (завдання 4)
        var location = "/api/TodoItems/by-ids?" +
            string.Join("&", createdItems.Select(x => $"ids={x.Id}"));

        return Created(location, createdItems);
    }

    // Завдання 1. Видалення запису
    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

    // Завдання 1, 2. Перетворення сутності в DTO (з новими полями)
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
       new TodoItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete,
           DueDate = todoItem.DueDate,
           Priority = todoItem.Priority
       };

    // Завдання 2, 5. Перетворення DTO в сутність (Id генерується базою даних)
    private static TodoItem DTOToItem(TodoItemDTO todoDTO) =>
       new TodoItem
       {
           Name = todoDTO.Name,
           IsComplete = todoDTO.IsComplete,
           DueDate = todoDTO.DueDate!.Value,
           Priority = todoDTO.Priority
       };
}
