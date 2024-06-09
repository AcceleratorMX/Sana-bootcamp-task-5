using MyTodoList.Data.Models;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Data.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int JobId { get; set; }
    public Job? Job { get; set; }
    public RepositoryTypes CurrentRepositoryType { get; set; }
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
}