using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Task.Data;

namespace SmartHotel.Services.Task.Controllers
{
    [Route("[controller]")]
    public class TasksController : Controller
    {
        private readonly TaskDbContext db;
        public TasksController(TaskDbContext db)
        {
            this.db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await this.db.Tasks
                .OrderByDescending(t => t.Submitted)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("/pending")]
        public async Task<IActionResult> GetPendings()
        {
            var tasks = await this.db.Tasks
                .Where(t => t.Resolved == false)
                .OrderByDescending(t => t.Submitted)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPut("/resolved/{id}")]
        public async Task<IActionResult> ResolveTask(int id)
        {
            var task = await this.db.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            task.Resolved = true;

            await this.db.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPut("/pending/{id}")]
        public async Task<IActionResult> UnresolveTask(int id)
        {
            var task = await this.db.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            task.Resolved = false;

            await this.db.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangeStatus([FromBody]Task.Domain.Task newTask, int id)
        {
            var task = await this.db.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();

            task.Resolved = newTask.Resolved;

            await this.db.SaveChangesAsync();

            return Ok(task);
        }
    }
}