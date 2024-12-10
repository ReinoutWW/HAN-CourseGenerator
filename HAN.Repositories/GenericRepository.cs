using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : BaseEntity
{
    private DbSet<T> Entity => context.Set<T>();

    public void Add(T entity)
    {
        Entity.Add(entity);   
        context.SaveChanges(); 
    }

    public void Delete(T entity)
    {
        Entity.Remove(entity);
        context.SaveChanges();
    }

    public List<T> GetAll()
    {
        return Entity.ToList();   
    }

    public T? GetById(int id)
    {
        var entity = context.Find<T>(id);
        return entity;
    }

    public void Update(T entity)
    {
        context.Update(entity);
        context.SaveChanges(); 
    }
}