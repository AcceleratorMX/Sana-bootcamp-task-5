using Dapper;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories.Sql;

public class JobRepositorySql(DatabaseService databaseService) : IRepository<Job, int>
{
    public async Task CreateAsync(Job job)
    {
        using var db = databaseService.OpenConnection();
        const string query = "INSERT INTO Jobs (Name, CategoryId, IsDone) VALUES (@Name, @CategoryId, @IsDone)";
        await db.ExecuteAsync(query, job);
    }
    
    public async Task<Job> GetByIdAsync(int id)
    {
        using var db = databaseService.OpenConnection();
        const string query = "SELECT Id, Name, CategoryId, IsDone FROM Jobs WHERE Id = @Id ";
        return await db.QueryFirstOrDefaultAsync<Job>(query, new { Id = id }) ?? 
               throw new NullReferenceException($"Job with id {id} not found!");
    }
    
    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        using var connection = databaseService.OpenConnection();
    
        var jobs = (await connection.QueryAsync<Job>("SELECT Id, Name, CategoryId, IsDone FROM Jobs")).ToList();
        var categories = (await connection.QueryAsync<Category>("SELECT Id, Name FROM Categories")).ToList();
    
        foreach (var job in jobs)
        {
            job.Category = categories.FirstOrDefault(c => c.Id == job.CategoryId) ??
                           throw new Exception($"Job with id {job.Id} has invalid category id {job.CategoryId}");
        }
    
        return jobs;
    }
    
    public async Task UpdateAsync(Job job)
    {
        using var db = databaseService.OpenConnection();
        const string query = "UPDATE Jobs SET Name = @Name, CategoryId = @CategoryId, IsDone = @IsDone WHERE Id = @Id";
        await db.ExecuteAsync(query, job);
    }
    
    public async Task DeleteAsync(int id)
    {
        using var db = databaseService.OpenConnection();
        await db.ExecuteAsync("DELETE FROM Jobs WHERE Id = @Id", new { Id = id });
    }
}