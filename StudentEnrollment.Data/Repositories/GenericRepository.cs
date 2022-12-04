using Microsoft.EntityFrameworkCore;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DataContext _db;

    public GenericRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(int? id)
    {
        var entity = await GetAsync(id);
        _db.Set<T>().Remove(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> Exists(int id)
    {
        return await _db.Set<T>().AnyAsync(q => q.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<T>? GetAsync(int? id)
    {
        var result = await _db.Set<T>().FindAsync(id);
        return result;
    }

    public async Task UpdateAsync(T entity)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync();
    }
}
