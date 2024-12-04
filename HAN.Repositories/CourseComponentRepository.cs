using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseComponentRepository(AppDbContext context) : RepositoryBase(context), ICourseComponentRepository
{
    public CourseComponent CreateCourseComponent(CourseComponent courseComponent)
    {
        if(courseComponent == null) 
        {
            throw new ArgumentException($"Course Component can not be null. Course: {nameof(courseComponent)}");
        }

        return Context.CourseComponents.Add(courseComponent).Entity;
    }

    public CourseComponent GetCourseComponentById(int id)
    {
        return Context.CourseComponents.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException();
    }

    public IEnumerable<CourseComponent> GetAllCourseComponents()
    {
        return [.. Context.CourseComponents];
    }

    public bool CourseComponentExists(int courseComponentId)
    {
        return Context.CourseComponents.Any(c => c.Id == courseComponentId);
    }

    public IEnumerable<Evl> GetEvlsByCourseComponentId(int id)
    {
        var courseComponent = Context.CourseComponents
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == id);
        
        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with id {id} does not exist. Please check the provided courseComponent ID.");
        
        return courseComponent.Evls;
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        var courseComponent = Context.CourseComponents.Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseComponentId);

        if (courseComponent == null)
            throw new KeyNotFoundException($"CourseComponent with ID {courseComponentId} not found.");

        var evl = Context.Evls.FirstOrDefault(e => e.Id == evlId);

        if (evl == null)
            throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

        if (courseComponent.Evls.Any(e => e.Id == evlId))
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with CourseComponent ID {courseComponentId}.");

        courseComponent.Evls.Add(evl);
        Context.SaveChanges();
    }
}

