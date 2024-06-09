using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.GraphQL.Types;

public sealed class JobType : ObjectGraphType<Job>
{
    public JobType(IRepository<Job, int> jobRepository)
    {
        Field(j => j.Id);
        Field(j => j.Name);
        Field(j => j.IsDone);
        Field(j => j.CategoryId, nullable: true);
        Field<CategoryType>("category").
            ResolveAsync(async context => await jobRepository.GetByIdAsync(context.Source.CategoryId));
    }
}
