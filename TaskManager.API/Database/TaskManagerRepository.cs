using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;
using System.Linq;

namespace TaskManager.API.Database
{
    public class TaskManagerRepository : ITaskManagerRepository
    {
        private readonly TaskManagerDbContext taskManagerDbContext;

        public TaskManagerRepository(TaskManagerDbContext taskManagerDbContext)
        {
            this.taskManagerDbContext = taskManagerDbContext;
        }

        public async Task DeleteAsync(TaskItem entity)
        {
            this.taskManagerDbContext.Tasks.Remove(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            var response = from task in taskManagerDbContext.Tasks
                           join user in taskManagerDbContext.User on task.Id equals user.TaskId into usr
                           from u in usr.DefaultIfEmpty()
                           select new TaskItemResponse
                           {
                               Id = task.Id,
                               Name = task.Name,
                               ParentTaskId = task.ParentTaskId,
                               EndDate = task.EndDate,
                               StartDate = task.StartDate,
                               EndTask = task.EndTask,
                               Priority = task.Priority,
                               Project = task.Project != null ? new Project { Id = task.Project.Id, Name = task.Project.Name } : null,
                               ProjectId = task.ProjectId,
                               FirstName = u != null ? u.FirstName : null,
                               LastName = u != null ? u.LastName : null,
                               UserId = u != null ? u.Id : 0,
                               User = u != null ? new User { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName } : null
                           };

            return await response.ToListAsync();
        }

        public async Task<TaskItem> GetAsync(int id)
        {
            return await taskManagerDbContext.Tasks.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> InsertAsync(TaskItem entity)
        {
            entity.Project = (entity.ProjectId != 0) ?
                taskManagerDbContext.Project.FirstOrDefault(x => x.Id == entity.ProjectId) : null;
            taskManagerDbContext.Tasks.Add(entity);

            return await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TaskItem entity)
        {
            taskManagerDbContext.Tasks.Update(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }
    }
}