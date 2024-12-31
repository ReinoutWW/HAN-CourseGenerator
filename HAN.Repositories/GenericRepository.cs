using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected DbSet<T> Entity => Context.Set<T>();
    protected readonly AppDbContext Context;

    protected GenericRepository(AppDbContext context)
    {
        Context = context;
    }
    
    public virtual void Add(T entity)
    {
        Entity.Add(entity);   
        Context.SaveChanges(); 
    }

    public void Delete(T entity)
    {
        Entity.Remove(entity);
        Context.SaveChanges();
    }

    public virtual List<T> GetAll()
    {
        return Entity.ToList();   
    }

    public virtual T? GetById(int id)
    {
        var entity = Context.Find<T>(id);
        return entity;
    }

    public bool Exists(int id)
    {
        return GetById(id) != null;
    }

    public virtual void Update(T entity)
    {
        var entry = Context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            var existingEntity = Context.Set<T>().Find(entity.Id);
            if (existingEntity != null)
            {
                Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                throw new ArgumentException("Entity with the specified ID does not exist.");
            }
        }

        Context.SaveChanges();
    }
}