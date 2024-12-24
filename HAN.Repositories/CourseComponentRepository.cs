using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = HAN.Data.Entities.File;

namespace HAN.Repositories;

public class CourseComponentRepository<TEntity>(AppDbContext context) : GenericRepository<TEntity>(context: context), ICourseComponentRepository<TEntity> where TEntity : CourseComponent
{
    private readonly AppDbContext _context = context;
    
    public override void Add(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.Evls = ResolveEvls(entity.Evls);

        Entity.Add(entity);
        _context.SaveChanges();
    }

    private List<Evl> ResolveEvls(List<Evl> evls)
    {
        if (evls == null)
            throw new ArgumentNullException(nameof(evls));

        var resolvedEvls = new List<Evl>();

        foreach (var evl in evls)
        {
            resolvedEvls.Add(FindOrAttachEvl(evl));
        }

        return resolvedEvls;
    }

    private Evl FindOrAttachEvl(Evl evl)
    {
        if (evl.Id <= 0)
        {
            throw new InvalidOperationException(
                "Cannot add a new Evl in this method. Only existing Evl references are allowed."
            );
        }

        var trackedEvl = _context.ChangeTracker.Entries<Evl>()
            .Where(e => e.Entity.Id == evl.Id)
            .Select(e => e.Entity)
            .FirstOrDefault();

        if (trackedEvl != null)
            return trackedEvl;

        var existingEvl = _context.Set<Evl>().Find(evl.Id);
        if (existingEvl == null)
        {
            throw new InvalidOperationException($"Evl with ID={evl.Id} does not exist in the database.");
        }

        _context.Set<Evl>().Attach(existingEvl);
        return existingEvl;
    }

    public IEnumerable<Evl> GetEvlsByCourseComponentId(int id)
    {
        var courseComponent = _context.CourseComponents
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == id);
        
        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with id {id} does not exist. Please check the provided courseComponent ID.");
        
        return courseComponent.Evls;
    }

    public List<File> GetFilesForCourseComponent(int courseComponentId)
    {
        if(!Exists(courseComponentId))
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");
        
        var files = _context.CourseComponents
            .Include(c => c.Files)
            .FirstOrDefault(c => c.Id == courseComponentId)
            ?.Files;

        return files ?? [];
    }

    public List<Evl> GetEvlsForCourseComponent(int courseComponentId)
    {
        if(!Exists(courseComponentId))
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");
        
        var evls = _context.CourseComponents
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseComponentId)
            ?.Evls;

        return evls ?? [];
    }

    public List<CourseComponent> GetAllCourseComponentsByEvlId(int evlId)
    {
        var courseComponents = _context.CourseComponents
            .Include(c => c.Evls)
            .Where(c => c.Evls.Any(e => e.Id == evlId))
            .ToList();
        
        return courseComponents;
    }

    public List<CourseComponent> GetAllCourseComponentByEvlIds(List<int> evlIds)
    {   
        var courseComponents = _context.CourseComponents
            .Include(c => c.Evls)
            .Where(c => c.Evls.Any(e => evlIds.Contains(e.Id)))
            .ToList();
        
        return courseComponents;
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        var courseComponent = _context.CourseComponents.Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        var evl = _context.Evls.FirstOrDefault(e => e.Id == evlId);

        if (evl == null)
            throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

        if (courseComponent.Evls.Any(e => e.Id == evlId))
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with CourseComponent ID {courseComponentId}.");

        courseComponent.Evls.Add(evl);
        _context.SaveChanges();
    }
    
    public void AddFileToCourseComponent(int courseComponentId, int fileId)
    {
        var courseComponent = _context.CourseComponents.Include(c => c.Files)
            .FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID {fileId} not found.");

        if (courseComponent.Files.Any(f => f.Id == fileId))
            throw new InvalidOperationException($"File with ID {fileId} is already associated with CourseComponent ID {courseComponentId}.");

        courseComponent.Files.Add(file);
        _context.SaveChanges();
    }
}

