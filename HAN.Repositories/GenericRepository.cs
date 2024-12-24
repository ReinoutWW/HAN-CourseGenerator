using HAN.Data;
using HAN.Data.EF;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected DbSet<T> Entity => Context.Set<T>();
    protected readonly AppDbContext Context;
    private readonly EntityCollectionSynchronizer _collectionSynchronizer;

    protected GenericRepository(AppDbContext context)
    {
        Context = context;
        _collectionSynchronizer = new EntityCollectionSynchronizer(context);
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

    public T? GetById(int id)
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
        var existingEntity = Context.Set<T>().Find(entity.Id);

        if (existingEntity == null)
            throw new ArgumentException("Entity with the specified ID does not exist.");

        var entry = Context.Entry(existingEntity);
        entry.CurrentValues.SetValues(entity);
        Context.SaveChanges();
    }
    
    protected void SynchronizeCollection<TU>(ICollection<TU> existing, ICollection<TU> updated) where TU : BaseEntity
    {
        _collectionSynchronizer.SynchronizeCollection(existing, updated);
    }
}