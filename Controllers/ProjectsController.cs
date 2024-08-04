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
    public class ProjectsController : ControllerBase
    {
        private readonly ApiDbContext? _context;


        // Initializes a new instance of the ProjectsController class with the specified database context.
        // Parameters:
        //   context: The database context to be used by the controller.
        // Throws:
        //   ArgumentNullException: If the context is null.

        public ProjectsController(ApiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetProjects()
        {

            var projects = await _context.Projects.Include(p => p.Tasks).ToListAsync();

            return Ok(projects);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> UpdateProject(int id, ProjectDto projectDto)
        {
            // verify that the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find project by id
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            // Update project with DTO
            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;
            existingProject.StartDate = projectDto.StartDate;
            existingProject.EndDate = projectDto.EndDate;

            // mark the state of the project as modified
            _context.Entry(existingProject).State = EntityState.Modified;

            // save the changes into the db
            await _context.SaveChangesAsync();

            return Ok("Project updated successfully.");

        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return BadRequest("Project not found");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}