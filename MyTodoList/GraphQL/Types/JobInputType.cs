using GraphQL.Types;

namespace MyTodoList.GraphQL.Types;

public sealed class JobInputType : InputObjectGraphType
{
    public JobInputType()
    {
        Field<IntGraphType>("id");
        Field<StringGraphType>("name");
        Field<IntGraphType>("categoryId");
        Field<BooleanGraphType>("isDone");
    }
}