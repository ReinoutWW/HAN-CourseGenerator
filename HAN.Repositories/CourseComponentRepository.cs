using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = HAN.Data.Entities.File;

namespace HAN.Repositories;

public class CourseComponentRepository<TEntity>(AppDbContext context) : GenericRepository<TEntity>(context), ICourseComponentRepository<TEntity> where TEntity : CourseComponent
{
    private readonly AppDbContext _context = context;

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
        
        var files = context.CourseComponents
            .Include(c => c.Files)
            .FirstOrDefault(c => c.Id == courseComponentId)
            ?.Files;

        return files ?? [];
    }

    public List<Evl> GetEvlsForCourseComponent(int courseComponentId)
    {
        if(!Exists(courseComponentId))
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");
        
        var evls = context.CourseComponents
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseComponentId)
            ?.Evls;

        return evls ?? [];
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

    public List<Lesson> GetLessons()
    {
        return _context.Set<CourseComponent>().OfType<Lesson>().ToList();
    }

    public List<Exam> GetExams()
    {
        return _context.Set<CourseComponent>().OfType<Exam>().ToList();
    }
}

