using GraphQL.Types;

namespace MyTodoList.GraphQL.Types;

public sealed class CategoryInputType : InputObjectGraphType
{
    public CategoryInputType()
    {
        Field<IntGraphType>("id");
        Field<StringGraphType>("name");
    }
}