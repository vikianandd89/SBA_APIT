using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Business
{
    public interface IProjectManager
    {
        Task<int> AddProjectAsync(Models.Project project);

        Task<IEnumerable<Models.ProjectResponse>> GetAllProjectAsync();

        Task<Models.Project> GetProjectAsync(int id);

        Task UpdateProjectAsync(int id, Models.Project project);

        Task DeleteAsync(int id);
    }
}
