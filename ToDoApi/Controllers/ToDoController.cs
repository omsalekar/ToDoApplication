using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.DTO;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ToDoController(ToDoDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddTask")]
        public IActionResult AddTask(TaskAddDTO dto)
        {
            var todo = new Model.ToDoModel
            {
                Task = dto.Task,
                IsCompleted = dto.IsCompleted
            };
            _context.ToDoItems.Add(todo);
            _context.SaveChanges();
            return Ok("Task Added");
        }

        [HttpGet("GetAllTasks")]

        public IActionResult GetAllTasks()
        {
            var tasks = _context.ToDoItems.ToList();
            return Ok(tasks);

        }

        [HttpPut("UpdateTask")]




        public IActionResult UpdateTask(TaskAddDTO dto)
        {
            var task = _context.ToDoItems.FirstOrDefault(p => p.Id == dto.Id);

            if (task == null)
            {
                return NotFound("Task Not Found");
            }

            task.Task = dto.Task;
            task.IsCompleted = dto.IsCompleted;
            _context.SaveChanges();
            return Ok("Task Updated");
        }

        [HttpDelete("DeleteTask")]

        public IActionResult DeleteTask(int id)
        {
            var task = _context.ToDoItems.FirstOrDefault(p => p.Id == id);

            if (task == null)
            {
                return NotFound("Task Not Found");
            }

            _context.ToDoItems.Remove(task);
            _context.SaveChanges();

            return Ok("Task Deleted");
        }

    }
}
