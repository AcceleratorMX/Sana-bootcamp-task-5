using System.Xml.Linq;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories.Xml;

public class CategoryRepositoryXml(XmlStorageService xmlStorageService) : IRepository<Category, int>
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var categories = from category in document.Descendants("Category")
            select new Category
            {
                Id = int.Parse(category.Attribute("id")?.Value ?? "0"),
                Name = category.Value
            };

        return categories.ToList();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var categoryElement = document.Root?.Elements("Category")
            .FirstOrDefault(e => int.Parse(e.Attribute("id")?.Value ?? "0") == id);
        if (categoryElement == null)
            throw new Exception($"Category with id {id} not found!");
        
        var category = new Category
        {
            Id = id,
            Name = categoryElement.Value
        };

        return category;
    }

    public async Task CreateAsync(Category category)
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var categoriesElement = document.Element("Categories") ??
                                throw new Exception("Categories element not found!");

        var lastCategoryId = categoriesElement.Descendants("Category").Select(c => (int?)c.Attribute("id")).Max() ?? 0;
        var newCategoryId = lastCategoryId + 1;

        var newCategoryElement = new XElement("Category",
            new XAttribute("id", newCategoryId),
            category.Name);

        categoriesElement.Add(newCategoryElement);
        await xmlStorageService.SaveCategoriesAsync(document);
    }

    public async Task UpdateAsync(Category category)
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var categoryElement = document.Root?.Elements("Category")
            .FirstOrDefault(e => int.Parse(e.Attribute("id")?.Value ?? "0") == category.Id)
                              ?? throw new Exception($"Job with id {category.Id} not found!");
        
        categoryElement.Value = category.Name;
        await xmlStorageService.SaveCategoriesAsync(document);
    }
    
    public async Task DeleteAsync(int id)
    {
        var document = await xmlStorageService.LoadCategoriesAsync();
        var categoryElement = document.Root?.Elements("Category")
            .FirstOrDefault(e => int.Parse(e.Attribute("id")?.Value ?? "0") == id);

        if (categoryElement == null)
            throw new Exception($"Category with id {id} not found!");

        categoryElement.Remove();
        await xmlStorageService.SaveCategoriesAsync(document);
    }
}