using GraphQL;
using GraphQL.Types;
using MyTodoList.GraphQL.Types;
using MyTodoList.Repositories;

namespace MyTodoList.GraphQL.Queries;

public sealed class RootQuery: ObjectGraphType
{
    public RootQuery(JobRepositorySwitcher repository)
    {
        Field<ListGraphType<JobType>>("jobs")
            .ResolveAsync(async context => await repository.GetJobs());
        
        Field<JobType>("job")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "id"}))
            .Resolve(context =>
            {
                var id = context.GetArgument<int>("id");
                return repository.GetJob(id);
            });

        Field<ListGraphType<CategoryType>>("categories")
            .ResolveAsync(async context => await repository.GetCategories());

        Field<StringGraphType>("repositoryType")
            .Resolve(context => repository.CurrentRepositoryType.ToString());
    }
}
