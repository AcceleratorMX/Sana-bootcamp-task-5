using GraphQL.Types;
using MyTodoList.Repositories;

namespace MyTodoList.GraphQL.Mutations;

public sealed class SwitchRepositoryTypeMutation : ObjectGraphType
{
    public SwitchRepositoryTypeMutation(JobRepositorySwitcher repositorySwitcher)
    {
        Field<StringGraphType>("switchToSql")
            .Resolve(context =>
            {
                repositorySwitcher.SwitchToSql();
                return "Switched to SQL repository";
            });

        Field<StringGraphType>("switchToXml")
            .Resolve(context =>
            {
                repositorySwitcher.SwitchToXml();
                return "Switched to XML repository";
            });
    }
}