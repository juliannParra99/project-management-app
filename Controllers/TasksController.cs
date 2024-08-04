using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using ProjectManagementApp.Models.DTOs;

namespace ProjectManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TasksController(ApiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectTask>>> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetTask(int id)
        {
            var task = await _context.Tasks.Include(t => t.ProjectId).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return Ok(task);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<ProjectTask>> PostTask(TaskDto taskDto)
        {
            // verify that the DTO is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // verify that the project exists
            var project = await _context.Projects.FindAsync(taskDto.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with ID {taskDto.ProjectId} not found.");
            }

            // make the task with the DTO
            var task = new ProjectTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Deadline = taskDto.Deadline,
                IsCompleted = taskDto.IsCompleted,
                ProjectId = taskDto.ProjectId
            };

            // add task to database
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // show created task
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            var project = await _context.Projects.FindAsync(taskDto.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with ID {taskDto.ProjectId} not found.");
            }

            // Update task with DTO
            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.Deadline = taskDto.Deadline;
            existingTask.IsCompleted = taskDto.IsCompleted;
            existingTask.ProjectId = taskDto.ProjectId;

            _context.Entry(existingTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
