using GraphQL.DataLoader;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services.DataLoader.Abstract;

namespace MyTodoList.GraphQL.Types;

public sealed class JobType : ObjectGraphType<Job>
{
    public JobType(IDataLoaderContextAccessor dataLoaderContextAccessor,
        ICustomDataLoader<int, Category> categoryDataLoader)
    {
        Field(j => j.Id);
        Field(j => j.Name);
        Field(j => j.IsDone);
        Field(j => j.CategoryId, nullable: true);

        Field<CategoryType, Category>("category")
            .ResolveAsync(context =>
            {
                var loader = dataLoaderContextAccessor.Context!.GetOrAddBatchLoader<int, Category>(
                    "GetCategoriesById", categoryDataLoader.LoadAsync);
                Console.WriteLine($"Resolving category {context.Source.CategoryId} for job {context.Source.Id}");
                return Task.FromResult(loader.LoadAsync(context.Source.CategoryId));
            });
    }
}