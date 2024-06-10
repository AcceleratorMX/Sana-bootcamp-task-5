using GraphQL.Types;
using MyTodoList.Data.Models;

namespace MyTodoList.GraphQL.Types;

public sealed class CategoryType : ObjectGraphType<Category>
{
    public CategoryType()
    {
        Field(c => c.Id);
        Field(c => c.Name);
    }
}
