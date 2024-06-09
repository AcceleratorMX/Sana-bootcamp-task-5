using GraphQL.Types;
using MyTodoList.GraphQL.Mutations;
using MyTodoList.GraphQL.Queries;

namespace MyTodoList.GraphQL.Schemes;

public class TodoSchema : Schema
{
    public TodoSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<RootQuery>();
        Mutation = provider.GetRequiredService<SwitchRepositoryType>();
    }
}