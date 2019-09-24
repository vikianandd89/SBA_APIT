using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Database
{
    public interface IProjectManagerRepository
    {
        Task<IEnumerable<ProjectResponse>> GetAllAsync();

        Task<Project> GetAsync(int id);

        Task<int> InsertAsync(Project entity);

        Task UpdateAsync(int id, Project entity);

        Task DeleteAsync(int id);
    }
}
