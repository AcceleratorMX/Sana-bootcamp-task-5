using MyTodoList.Data.Models;
using MyTodoList.Data.Services.DataLoader.Abstract;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Data.Services.DataLoader;

public class CategoryDataLoader(IRepositorySwitcher<Category, int> categoryRepository)
    : ICustomDataLoader<int, Category>
{
    public async Task<IDictionary<int, Category>> LoadAsync(IEnumerable<int> keys)
    {
        return (await categoryRepository.CurrentRepository.GetAllAsync())
            .Where(c => keys.Contains(c.Id)).ToDictionary(c => c.Id);
    }
}