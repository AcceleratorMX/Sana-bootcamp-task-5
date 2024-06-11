using GraphQL;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.GraphQL.Types;
using MyTodoList.Repositories;

namespace MyTodoList.GraphQL.Queries;

public sealed class RootQuery : ObjectGraphType
{
    public RootQuery(
        RepositorySwitcher<Job, int> jobRepository,
        RepositorySwitcher<Category, int> categoryRepository)
    {
        Field<ListGraphType<JobType>>("jobs")
            .ResolveAsync(async context => await jobRepository.GetAllAsync());
    
        Field<JobType>("job")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                return await jobRepository.GetByIdAsync(id);
            });
    
        Field<ListGraphType<CategoryType>>("categories")
            .ResolveAsync(async context => await categoryRepository.GetAllAsync());
        
        Field<CategoryType>("category")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                return await categoryRepository.GetByIdAsync(id);
            });
    
        Field<StringGraphType>("currentRepository")
            .Resolve(context => jobRepository.GetRepositoryType().ToString().ToUpper());
    }
}