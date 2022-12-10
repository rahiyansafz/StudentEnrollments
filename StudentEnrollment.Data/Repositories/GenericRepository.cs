using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Exceptions;
using StudentEnrollment.Data.Models;

namespace StudentEnrollment.Data.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DataContext _db;
    private readonly IMapper _mapper;

    public GenericRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
    {
        var entity = _mapper.Map<T>(source);

        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();

        return _mapper.Map<TResult>(entity);
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

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await _db.Set<T>().CountAsync();
        var items = await _db.Set<T>()
            .Skip(queryParameters.StartIndex)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TResult>
        {
            Items = items,
            PageNumber = queryParameters.PageNumber,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>()
    {
        return await _db.Set<T>()
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<T>? GetAsync(int? id)
    {
        var result = await _db.Set<T>().FindAsync(id);
        return result;
    }

    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        var result = await _db.Set<T>().FindAsync(id);

        if (result is null)
            throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");

        return _mapper.Map<TResult>(result);
    }

    public async Task UpdateAsync(T entity)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBase
    {
        if (id != source.Id)
            throw new BadRequestException("Invalid Id used in request");

        var entity = await GetAsync(id);

        if (entity is null)
            throw new NotFoundException(typeof(T).Name, id);

        _mapper.Map(source, entity);
        _db.Update(entity);
        await _db.SaveChangesAsync();
    }
}
