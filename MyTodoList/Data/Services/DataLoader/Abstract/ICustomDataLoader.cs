namespace MyTodoList.Data.Services.DataLoader.Abstract;

public interface ICustomDataLoader<TKey, TValue>
{
    Task<IDictionary<TKey, TValue>> LoadAsync(IEnumerable<TKey> keys);
}