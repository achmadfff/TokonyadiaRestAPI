using System.Linq.Expressions;

namespace TokonyadiaRestAPII.Repositories;

public interface IRepository<TEntity>
{
    Task<TEntity> SaveAsync(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> criteria);
    TEntity Attach(TEntity entity);
    Task<IEnumerable<TEntity>> SaveAll(IEnumerable<TEntity> entities);
    Task<TEntity?> FindById(Guid id);
    Task<TEntity?> FindById(Guid id, string[] includes);
    Task<TEntity?> Find(Expression<Func<TEntity, bool>> criteria);
    Task<TEntity?> Find(Expression<Func<TEntity, bool>> criteria, string[] includes);
    Task<IEnumerable<TEntity>> FindAll();
    Task<IEnumerable<TEntity>> FindAll(string[] includes);
    Task<IEnumerable<TEntity>> FindAll(int page, int size, string[]? includes);
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria);
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes);
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria, int page, int size);
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria, int page, int size, string[]? includes);
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> criteria, int? page, int? size, string[]? includes, Expression<Func<TEntity, object>>? orderBy, string direction);
    TEntity Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteAll(IEnumerable<TEntity> entities);
    Task<int> Count();
    Task<int> Count(Expression<Func<TEntity, bool>> criteria);
}