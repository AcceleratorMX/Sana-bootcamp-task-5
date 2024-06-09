using System.Xml.Linq;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories.Xml;

public class JobRepositoryXml(XmlStorageService xmlStorageService) : IRepository<Job, int>
{
    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        var categories = await GetCategories();
        var document = await xmlStorageService.LoadJobsAsync();
        var query = from job in document.Descendants("Job")
            select new Job
            {
                Id = int.Parse(job.Attribute("id")?.Value ?? throw new Exception("Job id not found!")),
                Name = job.Value,
                CategoryId = int.Parse(job.Attribute("categoryId")?.Value ?? "0"),
                IsDone = bool.Parse(job.Attribute("isDone")?.Value ?? "false"),
                Category = categories.FirstOrDefault(c => c.Id == int.Parse(job.Attribute("categoryId")?.Value ?? "0")) ??
                           throw new Exception($"Category with id {job.Attribute("categoryId")?.Value} not found!")
            };

        var jobs = query.ToList();
        return await Task.FromResult(jobs);
    }

    public async Task<Job> GetByIdAsync(int id)
    {
        var categories = await GetCategories();
        var document = await xmlStorageService.LoadJobsAsync();
        var jobElement = document.Root?.Elements("Job")
            .FirstOrDefault(e => int.Parse(e.Attribute("id")?.Value ?? "0") == id);

        if (jobElement == null)
            throw new Exception($"Job with id {id} not found!");

        var job = new Job
        {
            Id = id,
            Name = jobElement.Value,
            CategoryId = int.Parse(jobElement.Attribute("categoryId")?.Value ?? "0"),
            IsDone = bool.Parse(jobElement.Attribute("isDone")?.Value ?? "false")
        };
        
        job.Category = categories.FirstOrDefault(c => c.Id == job.CategoryId) ??
                       throw new Exception($"Category with id {job.CategoryId} not found!");

        return job;
    }

    public async Task CreateAsync(Job job)
    {
        var document = await xmlStorageService.LoadJobsAsync();
        var jobsElement = document.Element("Jobs") ??
                          throw new Exception("Jobs element not found!");

        var lastJobId = jobsElement.Descendants("Job").Select(j => (int?)j.Attribute("id")).Max() ?? 0;
        var newJobId = lastJobId + 1;

        var newJob = new XElement("Job",
            new XAttribute("id", newJobId),
            new XAttribute("categoryId", job.CategoryId),
            new XAttribute("isDone", job.IsDone),
            job.Name
        );

        jobsElement.Add(newJob);
        await xmlStorageService.SaveJobsAsync(document);
    }

    public async Task UpdateAsync(Job job)
    {
        var document = await xmlStorageService.LoadJobsAsync();
        var jobElement = document.Descendants("Job")
                             .FirstOrDefault(e => (int)e.Attribute("id")! == job.Id)
                         ?? throw new Exception($"Job with id {job.Id} not found!");

        jobElement.SetAttributeValue("isDone", job.IsDone);
        jobElement.SetAttributeValue("categoryId", job.CategoryId);
        await xmlStorageService.SaveJobsAsync(document);
    }

    public async Task DeleteAsync(int id)
    {
        var document = await xmlStorageService.LoadJobsAsync();
        var jobElement = document.Descendants("Job")
            .FirstOrDefault(e => (int)e.Attribute("id")! == id);

        if (jobElement == null)
            throw new Exception($"Job with id {id} not found!");

        jobElement.Remove();
        await xmlStorageService.SaveJobsAsync(document);
    }
    
    private async Task<IEnumerable<Category>> GetCategories()
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var query = from category in document.Descendants("Category")
            select new Category
            {
                Id = int.Parse(category.Attribute("id")?.Value ?? throw new Exception($"Category id {category.Value} not found!")),
                Name = category.Value
            };

        return query.ToList();
    }
}