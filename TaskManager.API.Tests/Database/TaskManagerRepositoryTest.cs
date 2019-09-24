using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Database;
using TaskManager.API.Models;
using TaskManager.API.Tests.Database.Helper;
using Xunit;

namespace TaskManager.API.Tests.Database
{
    public class TaskManagerRepositoryTest : IDisposable
    {
        IQueryable<User> userList = null;
        IQueryable<TaskItem> taskDetailsList = null;
        Mock<TaskManagerDbContext> mockContext = null;

        public TaskManagerRepositoryTest()
        {
        }

        [Fact]
        public async Task TestGetAll_ReturnsTwoTaskDetails()
        {
            SetUpMockData();

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            var taskDetails = await taskRepository.GetAllAsync();

            Assert.Equal(2, taskDetails.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {
            SetUpMockData();

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            var taskDetails = await taskRepository.GetAsync(2);

            Assert.IsType<TaskItem>(taskDetails);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            SetUpMockData();

            var taskRepository = new TaskManagerRepository(mockContext.Object);
            var taskDetail = new TaskItem() { Id = 1, Name = "Task 1 ", Priority = 10 };
            var mockSet = new Mock<DbSet<TaskItem>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskRepository.InsertAsync(taskDetail);

            mockSet.Verify(m => m.Add(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            var taskDetail = new TaskItem() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskItem>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            await taskRepository.UpdateAsync(1, taskDetail);

            mockSet.Verify(m => m.Update(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        private void SetUpMockData()
        {
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            mockContext = new Mock<TaskManagerDbContext>(contextOptions);
            userList = new List<User>().AsQueryable();

            taskDetailsList = new List<TaskItem>()
                {
                    new TaskItem() {Id = 1, Name ="Task 1 ", Priority = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) },
                    new TaskItem() {Id = 2, Name ="Task 2 ", Priority = 20, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) }
                }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskItem>>();
            var mockUserSet = new Mock<DbSet<User>>();

            mockSet.As<IAsyncEnumerable<TaskItem>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<TaskItem>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskItem>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockUserSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<User>(userList.GetEnumerator()));

            mockUserSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<User>(userList.Provider));

            mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            mockContext.Setup(m => m.User).Returns(mockUserSet.Object);
        }

        public void Dispose()
        {
        }
    }
}
