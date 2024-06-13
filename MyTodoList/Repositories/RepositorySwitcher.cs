using MyTodoList.Repositories.Abstract;

namespace MyTodoList.Repositories;

public class RepositorySwitcher<T, TId>(
    IRepository<T, TId> sqlRepository,
    IRepository<T, TId> xmlRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRepositorySwitcher<T, TId>
    where T : class
{
    private const string RepositoryTypeHeaderName = "Repository-Type";
    private RepositoryTypes _currentRepositoryType = RepositoryTypes.Sql;
    public IRepository<T, TId> CurrentRepository
    {
        get
        {
            UpdateCurrentRepositoryType();
            return _currentRepositoryType == RepositoryTypes.Xml ? xmlRepository : sqlRepository;
        }
    }

    private void UpdateCurrentRepositoryType()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.Request.Headers.TryGetValue(RepositoryTypeHeaderName, out var repositoryTypeHeader))
        {
            _currentRepositoryType = repositoryTypeHeader.ToString().ToUpperInvariant() switch
            {
                "SQL" => RepositoryTypes.Sql,
                "XML" => RepositoryTypes.Xml,
                _ => _currentRepositoryType
            };
        }
    }

    public RepositoryTypes GetRepositoryType() => _currentRepositoryType;
    public void SwitchToSql() => SetHeader(RepositoryTypes.Sql);
    public void SwitchToXml() => SetHeader(RepositoryTypes.Xml);
    private void SetHeader(RepositoryTypes repositoryType)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            httpContext.Request.Headers[RepositoryTypeHeaderName] = repositoryType.ToString().ToLowerInvariant();
            _currentRepositoryType = repositoryType;
        }
    }
}