using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories;

public class RepositorySwitcher<T, TId>(
    IRepository<T, TId> sqlRepository,
    IRepository<T, TId> xmlRepository)
    : IRepository<T, TId>
    where T : class
{
    private RepositoryTypes _currentRepositoryType = RepositoryTypes.Sql;

    private IRepository<T, TId> CurrentRepository =>
        _currentRepositoryType == RepositoryTypes.Xml ? xmlRepository : sqlRepository;

    public void SwitchToSql() => _currentRepositoryType = RepositoryTypes.Sql;
    public void SwitchToXml() => _currentRepositoryType = RepositoryTypes.Xml;
    
    public RepositoryTypes GetRepositoryType() => _currentRepositoryType;
    
    public Task<IEnumerable<T>> GetAllAsync() => CurrentRepository.GetAllAsync();
    public Task<T> GetByIdAsync(TId id) => CurrentRepository.GetByIdAsync(id);
    public Task CreateAsync(T entity) => CurrentRepository.CreateAsync(entity);
    public Task UpdateAsync(T entity) => CurrentRepository.UpdateAsync(entity);
    public Task DeleteAsync(TId id) => CurrentRepository.DeleteAsync(id);
}