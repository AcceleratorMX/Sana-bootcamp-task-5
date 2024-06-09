using MyTodoList.Enums;

namespace MyTodoList.Data.Services;

public class RepositoryTypeService
{
    public RepositoryTypes CurrentRepositoryType { get; set; } = RepositoryTypes.Sql;
}
