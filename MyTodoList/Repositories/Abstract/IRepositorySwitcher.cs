namespace MyTodoList.Repositories.Abstract;

public interface IRepositorySwitcher<T, TId> where T : class
{
    IRepository<T, TId> CurrentRepository { get; }
    void SwitchToSql();
    void SwitchToXml();
    RepositoryTypes GetRepositoryType();
}