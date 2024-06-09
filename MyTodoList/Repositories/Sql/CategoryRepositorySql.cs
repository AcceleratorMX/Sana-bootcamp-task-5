using Dapper;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories.Sql;

public class CategoryRepositorySql(DatabaseService databaseService) : IRepository<Category, int>
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var db = databaseService.OpenConnection();
        return await db.QueryAsync<Category>("SELECT Id, Name FROM Categories");
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        using var db = databaseService.OpenConnection();
        return await db.QueryFirstOrDefaultAsync("SELECT Id, Name FROM Categories WHERE Id = @Id", new { Id = id }) 
               ?? throw new NullReferenceException($"Category with id {id} not found!");
    }

    public async Task CreateAsync(Category category)
    {
        using var db = databaseService.OpenConnection();
        await db.ExecuteAsync("INSERT INTO Categories (Name) VALUES (@Name)", category);
    }

    public async Task UpdateAsync(Category category)
    {
        using var db = databaseService.OpenConnection();
        await db.ExecuteAsync("UPDATE Categories SET Name = @Name WHERE Id = @Id", category);
    }

    public async Task DeleteAsync(int id)
    {
        using var db = databaseService.OpenConnection();
        await db.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
    }
}