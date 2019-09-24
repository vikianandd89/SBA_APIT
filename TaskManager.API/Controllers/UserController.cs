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
    [Route("api/User")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserManager userManager;
        private readonly ILogger<UserController> logger;

        public UserController(
            IUserManager userManager,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        // GET api/User
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                this.logger.LogInformation("Entering into Get all users method");
                return Ok(await userManager.GetAllUserAsync());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // GET api/User/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                this.logger.LogInformation($"Getting user detail for {id}");
                return Ok(await userManager.GetUserAsync(id));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Issue with server, please try again later");
            }
        }

        // POST api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    this.logger.LogInformation("Provide valid user detail");
                    return BadRequest();
                }

                await userManager.AddUserAsync(user);
                this.logger.LogInformation($"User {user.Id} created successfully");

                return Ok($"Task {user.Id} created successfully");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // PUT api/User/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            try
            {
                if (user == null || user.Id != id)
                {
                    this.logger.LogInformation("Provide valid user item detail");
                    return BadRequest("Provide a valid task");
                }

                await userManager.UpdateUserAsync(id, user);
                this.logger.LogInformation($"Task {user.FirstName + " " + user.LastName} updated successfully");

                return Ok($"Task {user.FirstName + " " + user.LastName} updated successfully");
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
                this.logger.LogInformation($"Getting user detail for {id}");
                await userManager.DeleteUserAsync(id);

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