using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using ProjectManagementApp.Models.DTOs;

namespace ProjectManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //protect all the routes; needed to get the JWT token with role of manager
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "manager")]
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
        public async Task<ActionResult<Project>> PostProject(ProjectDto projectDto)
        {

            // Check if the DTO is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new Project entity from the DTO
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate
            };

            // Add the new project to the database
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Return the newly created project
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
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