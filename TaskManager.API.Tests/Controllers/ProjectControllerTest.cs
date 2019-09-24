using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TaskManager.API.Business;
using TaskManager.API.Controllers;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.API.Tests.Controllers
{
    public class ProjectControllerTest : IDisposable
    {
        public ILogger<ProjectController> Logger { get; private set; }

        public ProjectControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<ProjectController>();
        }

        [Fact]
        public async Task TestGetAllAsync_VerifyServiceReturnOkStatus()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            var projectList = new List<ProjectResponse>()
            {
                new ProjectResponse() {Id = 1, Name ="Project 1 ", Priority = 10},
                new ProjectResponse() {Id = 2, Name ="Project 2 ", Priority = 20},
            };

            mockManageTask.Setup(manage => manage.GetAllProjectAsync()).Returns(Task.FromResult<IEnumerable<ProjectResponse>>(projectList));

            var statusResult = await taskRepository.Get();

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as List<ProjectResponse>;
            Assert.Equal(2, taskDetailsResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            mockManageTask.Setup(manage => manage.GetAllProjectAsync()).Throws(new Exception());

            var statusResult = await taskRepository.Get();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckTaskDetails()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            var project =
                new Project() { Id = 1, Name = "Project 1 ", Priority = 10 };

            mockManageTask.Setup(manage => manage.GetProjectAsync(1)).Returns(Task.FromResult<Project>(project));

            var statusResult = await taskRepository.Get(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as Project;
            Assert.IsType<Project>(taskDetailsResult);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            mockManageTask.Setup(manage => manage.GetProjectAsync(1)).Throws(new Exception());

            var statusResult = await taskRepository.Get(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckTaskId()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            var project = new Project() { Id = 1001, Name = "Project 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.AddProjectAsync(project)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.Post(project);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("1001", (statusResult as OkObjectResult).Value.ToString());
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);

            var statusResult = await taskRepository.Post(null);

            Assert.NotNull(statusResult as BadRequestResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<IProjectManager>();
            var taskRepository = new ProjectController(mockManageTask.Object, this.Logger);
            var project = new Project() { Id = 1001, Name = "Project 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.AddProjectAsync(project)).Throws(new Exception());

            var statusResult = await taskRepository.Post(project);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        public void Dispose()
        {
        }
    }
}
