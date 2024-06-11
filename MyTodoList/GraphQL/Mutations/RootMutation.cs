using GraphQL;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.GraphQL.Types;
using MyTodoList.Repositories;
using MyTodoList.Repositories.Abstract;

namespace MyTodoList.GraphQL.Mutations;

public sealed class RootMutation : ObjectGraphType
{
    public RootMutation(
        RepositorySwitcher<Job, int> job,
        RepositorySwitcher<Category, int> category)
    {
        Field<StringGraphType>("createJob")
            .Arguments(new QueryArguments(
                new QueryArgument<JobInputType> { Name = "job" }))
            .ResolveAsync(async context =>
            {
                var newJob = context.GetArgument<Job>("job");
                await job.CreateAsync(newJob);
                return $"{newJob.Name} successfully created";
            });

        Field<StringGraphType>("changeProgress")
            .Arguments(new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<BooleanGraphType> { Name = "isDone" }))
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                var isDone = context.GetArgument<bool>("isDone");

                var jobToUpdate = await job.GetByIdAsync(id);
                jobToUpdate.IsDone = isDone;
                await job.UpdateAsync(jobToUpdate);

                return $"Progress of job with id {id} successfully changed";
            });
        
        Field<StringGraphType>("deleteJob")
            .Arguments(new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                await job.DeleteAsync(id);
                return $"Job with id {id} successfully deleted";
            });
        
        Field<StringGraphType>("createCategory")
            .Arguments(new QueryArguments(
                new QueryArgument<CategoryInputType> { Name = "category" }))
            .ResolveAsync(async context =>
            {
                var newCategory = context.GetArgument<Category>("category");
                await category.CreateAsync(newCategory);
                return $"{newCategory.Name} successfully created";
            });
        
        Field<StringGraphType>("deleteCategory")
            .Arguments(new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                await category.DeleteAsync(id);
                return $"Category with id {id} successfully deleted";
            });

        Field<StringGraphType>("switchRepository")
            .Resolve(context =>
            {
                if (job.GetRepositoryType() == RepositoryTypes.Sql || job.GetRepositoryType() == RepositoryTypes.Sql)
                {
                    job.SwitchToXml();
                    category.SwitchToXml();
                    return "Switched to XML";
                }
                else
                {
                    job.SwitchToSql();
                    category.SwitchToSql();
                    return "Switched to SQL";
                }
            });
    }
}