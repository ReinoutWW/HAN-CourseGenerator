using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseRepository(AppDbContext context) : RepositoryBase(context), ICourseRepository
{
    public Course CreateCourse(Course course)
    {
        if(course == null) 
        {
            throw new ArgumentException($"Course can not be null. Course: {nameof(course)}");
        }

        return Context.Courses.Add(course).Entity;
    }

    public IEnumerable<Course> GetAllCourses()
    {
        return [.. Context.Courses];
    }

    public IEnumerable<Evl> GetEvlsByCourseId(int id)
    {
        var course = Context.Courses
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == id);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id {id} does not exist. Please check the provided course ID.");
        
        return course.Evls;
    }

    public bool CourseExists(int courseId)
    {
        return Context.Courses.Any(c => c.Id == courseId);
    }

    public void AddEvlToCourse(int courseId, int evlId)
    {
        var course = Context.Courses.Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException($"Course with ID {courseId} not found.");

        var evl = Context.Evls.FirstOrDefault(e => e.Id == evlId);

        if (evl == null)
            throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

        if (course.Evls.Any(e => e.Id == evlId))
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with Course ID {courseId}.");

        course.Evls.Add(evl);
        Context.SaveChanges();
    }

    public Course GetCourseById(int id)
    {
        return Context.Courses.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException();
    }
}