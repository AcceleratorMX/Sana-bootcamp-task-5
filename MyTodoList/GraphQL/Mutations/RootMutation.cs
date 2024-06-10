using GraphQL.Types;
using MyTodoList.Data.Models;
using MyTodoList.Repositories;

namespace MyTodoList.GraphQL.Mutations;

public sealed class RootMutation : ObjectGraphType
{
    public RootMutation(
        RepositorySwitcher<Job, int> jobRepositorySwitcher,
        RepositorySwitcher<Category, int> categoryRepositorySwitcher)
    {
        Field<StringGraphType>("switchToSql")
            .Resolve(context =>
            {
                jobRepositorySwitcher.SwitchToSql();
                categoryRepositorySwitcher.SwitchToSql();
                return "Switched to SQL";
            });
    
        Field<StringGraphType>("switchToXml")
            .Resolve(context =>
            {
                jobRepositorySwitcher.SwitchToXml();
                categoryRepositorySwitcher.SwitchToXml();
                return "Switched to XML";
            });
    }
}