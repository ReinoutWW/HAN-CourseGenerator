using HAN.Data;
using HAN.Data.Entities;

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

    public Course GetCourseById(int id)
    {
        return Context.Courses.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException();
    }
}