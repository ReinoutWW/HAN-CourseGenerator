using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = HAN.Data.Entities.File;

namespace HAN.Repositories;

public class CourseComponentRepository<TEntity>(AppDbContext context)
    : GenericRepository<TEntity>(context: context), ICourseComponentRepository<TEntity> where TEntity : CourseComponent
{
    private readonly AppDbContext _context = context;

    public override void Add(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        entity.EvlIds = ValidateAndResolveEvlIds(entity.EvlIds);
        entity.FileIds = ValidateAndResolveFileIds(entity.FileIds);

        Entity.Add(entity);
        _context.SaveChanges();
    }

    public List<File> GetFilesForCourseComponent(int courseComponentId)
    {
        if (!Exists(courseComponentId))
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        var fileIds = _context.CourseComponents
            .Where(c => c.Id == courseComponentId)
            .Select(c => c.FileIds)
            .FirstOrDefault() ?? new List<int>();

        return _context.Files.Where(f => fileIds.Contains(f.Id)).ToList();
    }

    public List<Evl> GetEvlsForCourseComponent(int courseComponentId)
    {
        var evlIds = _context.CourseComponents
            .Where(c => c.Id == courseComponentId)
            .Select(c => c.EvlIds)
            .FirstOrDefault();

        if (evlIds == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        return _context.Evls.Where(e => evlIds.Contains(e.Id)).ToList();
    }

    public List<CourseComponent> GetAllCourseComponentsByEvlId(int evlId)
    {
        return _context.CourseComponents
            .Where(c => c.EvlIds.Contains(evlId))
            .ToList();
    }

    public List<CourseComponent> GetAllCourseComponentByEvlIds(List<int> evlIds)
    {
        if (evlIds == null || !evlIds.Any())
            return [];

        return _context.CourseComponents
            .AsEnumerable() // Fetch all components and evaluate in memory
            .Where(c => c.EvlIds.Any(id => evlIds.Contains(id)))
            .ToList();
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        var courseComponent = _context.CourseComponents.FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        if (!courseComponent.EvlIds.Contains(evlId))
        {
            if (!_context.Evls.Any(e => e.Id == evlId))
                throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

            courseComponent.EvlIds.Add(evlId);
            _context.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException(
                $"Evl with ID {evlId} is already associated with CourseComponent ID {courseComponentId}.");
        }
    }

    public void AddFileToCourseComponent(int courseComponentId, int fileId)
    {
        var courseComponent = _context.CourseComponents.FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        if (!courseComponent.FileIds.Contains(fileId))
        {
            if (!_context.Files.Any(f => f.Id == fileId))
                throw new KeyNotFoundException($"File with ID {fileId} not found.");

            courseComponent.FileIds.Add(fileId);
            _context.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException(
                $"File with ID {fileId} is already associated with CourseComponent ID {courseComponentId}.");
        }
    }
    
    private List<int> ValidateAndResolveEvlIds(List<int> evlIds)
    {
        if (evlIds == null)
            throw new ArgumentNullException(nameof(evlIds));

        foreach (var id in evlIds)
        {
            var evlExists = _context.Evls.Any(e => e.Id == id);
            if (!evlExists)
                throw new InvalidOperationException($"Evl with Id {id} not found.");
        }

        return evlIds;
    }

    private List<int> ValidateAndResolveFileIds(List<int> fileIds)
    {
        if (fileIds == null)
            throw new ArgumentNullException(nameof(fileIds));

        foreach (var id in fileIds)
        {
            var fileExists = _context.Files.Any(e => e.Id == id);
            if (!fileExists)
                throw new InvalidOperationException($"File with Id {id} not found.");
        }

        return fileIds;
    }
}