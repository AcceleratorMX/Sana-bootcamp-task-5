using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Enums;
using MyTodoList.Interfaces;

namespace MyTodoList.Repositories;

public class JobRepositorySwitcher(
    IJobRepository sqlRepository,
    IJobRepository xmlRepository,
    RepositoryTypeService repositoryTypeService,
    ILogger<JobRepositorySwitcher> logger)
    : IJobRepository
{
    private readonly ILogger<JobRepositorySwitcher> _logger = logger;
    
    private IJobRepository CurrentRepository
    {
        get
        {
            return repositoryTypeService.CurrentRepositoryType switch
            {
                RepositoryTypes.Xml => xmlRepository,
                _ => sqlRepository,
            };
        }
    }
    
    public RepositoryTypes CurrentRepositoryType => repositoryTypeService.CurrentRepositoryType;
    public void SwitchToSql() => repositoryTypeService.CurrentRepositoryType = RepositoryTypes.Sql;
    public void SwitchToXml() => repositoryTypeService.CurrentRepositoryType = RepositoryTypes.Xml;

    public Task<int> AddJob(Job job) => CurrentRepository.AddJob(job);
    public Task<IEnumerable<Job>> GetJobs() => CurrentRepository.GetJobs();
    public Task<Job> GetJob(int id) => CurrentRepository.GetJob(id);
    public Task<int> UpdateJob(Job job) => CurrentRepository.UpdateJob(job);
    public Task<int> DeleteJob(int id) => CurrentRepository.DeleteJob(id);
    public Task<IEnumerable<Category>> GetCategories() => CurrentRepository.GetCategories();
}
