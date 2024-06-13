using GraphQL.DataLoader;
using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Data.Services.DataLoader.Abstract;

namespace MyTodoList.GraphQL.Types;

public sealed class JobType : ObjectGraphType<Job>
{
    public JobType(IDataLoaderContextAccessor accessor,
        ICustomDataLoader<int, Category> dataLoader)
    {
        Field(j => j.Id);
        Field(j => j.Name);
        Field(j => j.IsDone);
        Field(j => j.CategoryId, nullable: true);

        Field<CategoryType, Category>("category")
            .ResolveAsync(context => accessor.Context!
                .GetOrAddBatchLoader<int, Category>("GetCategoriesById", async keys => 
                    await dataLoader.LoadAsync(keys)).LoadAsync(context.Source.CategoryId));
    }
}