using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models;

namespace StudentEnrollment.Data.Contracts;
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<bool> DeleteAsync(int? id);
    Task UpdateAsync(T entity);
    Task<bool> Exists(int id);

    // Paged Result
    Task<TResult> GetAsync<TResult>(int? id);
    Task<IEnumerable<TResult>> GetAllAsync<TResult>();
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<TResult> AddAsync<TSource, TResult>(TSource source);
    Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBase;
}

public interface IBase
{
    int Id { get; set; }
}