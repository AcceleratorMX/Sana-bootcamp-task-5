using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories;

public class RepositorySwitcher<T, TId>(
    IRepository<T, TId> sqlRepository,
    IRepository<T, TId> xmlRepository)
    : IRepositorySwitcher<T, TId>
    where T : class
{
    private RepositoryTypes _currentRepositoryType = RepositoryTypes.Sql;

    public IRepository<T, TId> CurrentRepository =>
        _currentRepositoryType == RepositoryTypes.Xml ? xmlRepository : sqlRepository;
    
    public RepositoryTypes GetRepositoryType() => _currentRepositoryType;
    public void SwitchToSql() => _currentRepositoryType = RepositoryTypes.Sql;
    public void SwitchToXml() => _currentRepositoryType = RepositoryTypes.Xml;
}