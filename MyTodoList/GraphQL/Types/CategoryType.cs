using GraphQL.Types;
using MyTodoList.Data.Models;

namespace MyTodoList.GraphQL.Types;

public sealed class CategoryType : ObjectGraphType<Category>
{
    public CategoryType()
    {
        Field(x => x.Id).Description("Category Id");
        Field(x => x.Name).Description("Category Name");
    }
}
