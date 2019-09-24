using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Database
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly TaskManagerDbContext taskManagerDbContext;

        public UserManagerRepository(TaskManagerDbContext taskManagerDbContext)
        {
            this.taskManagerDbContext = taskManagerDbContext;
        }

        public async Task DeleteAsync(int id)
        {
            var deleteEntity = taskManagerDbContext.User.FirstOrDefaultAsync(s => s.Id == id);

            this.taskManagerDbContext.User.Remove(deleteEntity.Result);

            await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this.taskManagerDbContext.User.AsNoTracking<User>().ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await taskManagerDbContext.User.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> InsertAsync(User entity)
        {
            entity.Project = (entity.ProjectId != 0) ?
                taskManagerDbContext.Project.FirstOrDefault(x => x.Id == entity.ProjectId) : null;
            entity.Task = (entity.TaskId != 0) ?
                taskManagerDbContext.Tasks.FirstOrDefault(x => x.Id == entity.TaskId) : null;
            taskManagerDbContext.User.Add(entity);
 
            return await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, User entity)
        {
            taskManagerDbContext.User.Update(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }
    }
}
