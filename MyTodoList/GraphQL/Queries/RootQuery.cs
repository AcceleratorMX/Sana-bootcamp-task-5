using GraphQL;
using GraphQL.Types;
using MyTodoList.GraphQL.Types;
using MyTodoList.Repositories;

namespace MyTodoList.GraphQL.Queries;

public sealed class RootQuery: ObjectGraphType
{
    // public RootQuery(RepositorySwitcher repository)
    // {
    //     Field<ListGraphType<JobType>>("jobs")
    //         .ResolveAsync(async context => await repository.GetAllAsync());
    //     
    //     Field<JobType>("job")
    //         .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "id"}))
    //         .ResolveAsync(async context =>
    //         {
    //             var id = context.GetArgument<int>("id");
    //             return await repository.GetByIdAsync(id);
    //         });
    //
    //     Field<ListGraphType<CategoryType>>("categories")
    //         .ResolveAsync(async context => await repository.GetAllAsync());
    //
    //     Field<StringGraphType>("repositoryType")
    //         .Resolve(context => repository.CurrentRepositoryType.ToString());
    // }
}
