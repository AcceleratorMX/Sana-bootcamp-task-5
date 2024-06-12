using MyTodoList.Data.Models;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Data.ViewModels;

public class TodoViewModel
{
    public Job Job { get; set; } = null!;
    public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
    public RepositoryTypes CurrentRepositoryType { get; set; }
}