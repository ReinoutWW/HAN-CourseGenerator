using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseComponentRepository(AppDbContext context) : GenericRepository<CourseComponent>(context), ICourseComponentRepository
{
    public IEnumerable<Evl> GetEvlsByCourseComponentId(int id)
    {
        var courseComponent = context.CourseComponents
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == id);
        
        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with id {id} does not exist. Please check the provided courseComponent ID.");
        
        return courseComponent.Evls;
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        var courseComponent = context.CourseComponents.Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        var evl = context.Evls.FirstOrDefault(e => e.Id == evlId);

        if (evl == null)
            throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

        if (courseComponent.Evls.Any(e => e.Id == evlId))
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with CourseComponent ID {courseComponentId}.");

        courseComponent.Evls.Add(evl);
        context.SaveChanges();
    }
}

