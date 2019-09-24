using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.API.Business;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Route("api/Project")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IProjectManager projectManager;
        private readonly ILogger<ProjectController> logger;

        public ProjectController(
            IProjectManager projectManager,
            ILogger<ProjectController> logger)
        {
            this.projectManager = projectManager;
            this.logger = logger;
        }

        // GET api/Project
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                this.logger.LogInformation("Entering into Get all projects method");
                return Ok(await projectManager.GetAllProjectAsync());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // GET api/Project/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                this.logger.LogInformation($"Getting project detail for {id}");
                return Ok(await projectManager.GetProjectAsync(id));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Issue with server, please try again later");
            }
        }

        // POST api/Project
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Project project)
        {
            try
            {
                if (project == null)
                {
                    this.logger.LogInformation("Provide valid task item detail");
                    return BadRequest();
                }

                await projectManager.AddProjectAsync(project);
                this.logger.LogInformation($"Task {project.Id} created successfully");

                return Ok(project.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // PUT api/Project/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Project project)
        {
            try
            {
                if (project == null || project.Id != id)
                {
                    this.logger.LogInformation("Provide valid project detail");
                    return BadRequest("Provide a valid project");
                }

                await projectManager.UpdateProjectAsync(id, project);
                this.logger.LogInformation($"Project { project.Name } updated successfully");

                return Ok($"Project { project.Name } updated successfully");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // Delete api/User/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                this.logger.LogInformation($"Delete user detail for {id}");
                await projectManager.DeleteAsync(id);

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Issue with server, please try again later");
            }
        }
    }
}