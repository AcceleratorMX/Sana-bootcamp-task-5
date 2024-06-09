using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Interfaces;

namespace MyTodoList.GraphQL.Types;

public sealed class JobType : ObjectGraphType<Job>
{
    public JobType(IJobRepository jobRepository)
    {
        Field(j => j.Id);
        Field(j => j.Name);
        Field(j => j.IsDone);
        Field(j => j.CategoryId, nullable: true);
        Field<CategoryType>("category").Resolve(context => jobRepository.GetCategory(context.Source.CategoryId));
    }
}
