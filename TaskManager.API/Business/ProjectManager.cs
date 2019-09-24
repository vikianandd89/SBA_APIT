using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Database;
using TaskManager.API.Models;

namespace TaskManager.API.Business
{
    public class ProjectManager : IProjectManager
    {
        private readonly IProjectManagerRepository projectManagerRepository;
        private readonly ILogger<ProjectManager> logger;

        public ProjectManager(IProjectManagerRepository projectManagerRepository,
            ILogger<ProjectManager> logger)
        {
            this.logger = logger;
            this.projectManagerRepository = projectManagerRepository;
        }

        public async Task<int> AddProjectAsync(Project project)
        {
            return await projectManagerRepository.InsertAsync(project);
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllProjectAsync()
        {
            return await projectManagerRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await projectManagerRepository.GetAsync(id);
        }

        public async Task UpdateProjectAsync(int id, Project project)
        {
            await this.projectManagerRepository.UpdateAsync(id, project);
        }

        public async Task DeleteAsync(int id)
        {
            await this.projectManagerRepository.DeleteAsync(id);
        }
    }
}
