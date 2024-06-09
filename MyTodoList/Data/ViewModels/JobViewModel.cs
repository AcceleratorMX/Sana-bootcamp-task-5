using MyTodoList.Data.Models;

namespace MyTodoList.Data.ViewModels;

public class JobViewModel
{
    public Job NewJob { get; set; } = null!;
    public IEnumerable<Job> Jobs { get; set; } = null!;
}