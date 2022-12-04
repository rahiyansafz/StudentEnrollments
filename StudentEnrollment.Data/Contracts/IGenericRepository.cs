using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Contracts;
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<bool> DeleteAsync(int? id);
    Task UpdateAsync(T entity);
    Task<bool> Exists(int id);
}
