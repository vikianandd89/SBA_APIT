using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;
using System.Linq;

namespace TaskManager.API.Database
{
    public class ProjectManagerRepository : IProjectManagerRepository
    {
        private readonly TaskManagerDbContext taskManagerDbContext;

        public ProjectManagerRepository(TaskManagerDbContext taskManagerDbContext)
        {
            this.taskManagerDbContext = taskManagerDbContext;
        }

        public async Task DeleteAsync(Project entity)
        {
            this.taskManagerDbContext.Project.Remove(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
        {
            var response = from project in taskManagerDbContext.Project
                           join user in taskManagerDbContext.User on project.Id equals user.ProjectId into usr
                           join task in taskManagerDbContext.Tasks on project.Id equals task.ProjectId into tsk
                           from u in usr.DefaultIfEmpty()
                           select new ProjectResponse()
                           {
                               Id = project.Id,
                               Name = project.Name,
                               Priority = project.Priority,
                               StartDate = project.StartDate,
                               EndDate = project.EndDate,
                               UserId = u != null ? u.Id : 0,
                               TaskCount = tsk.Count(),
                               User = u != null ? new User { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName } : null
                           };

            return await response.ToListAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            return await taskManagerDbContext.Project.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> InsertAsync(Project entity)
        {
            taskManagerDbContext.Project.Add(entity);

            return await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Project entity)
        {
            taskManagerDbContext.Project.Update(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deleteEntity = taskManagerDbContext.Project.FirstOrDefault(x => x.Id == id);

            taskManagerDbContext.Project.Remove(deleteEntity);

            await taskManagerDbContext.SaveChangesAsync();
        }
    }
}