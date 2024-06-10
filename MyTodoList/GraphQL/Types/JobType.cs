using GraphQL.Types;
using MyTodoList.Data.Models;

namespace MyTodoList.GraphQL.Types;

public sealed class JobType : ObjectGraphType<Job>
{
    public JobType()
    {
        Field(j => j.Id);
        Field(j => j.Name);
        Field(j => j.IsDone);
        Field(j => j.CategoryId, nullable: true);
        Field<CategoryType>("category");
    }
}
