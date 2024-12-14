using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseRepository(AppDbContext context) : GenericRepository<Course>(context), ICourseRepository
{
    private readonly AppDbContext _context = context;

    public IEnumerable<Evl> GetEvlsByCourseId(int id)
    {
        var course = _context.Courses
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == id);
        
        if (course == null)
            throw new KeyNotFoundException($"Course with id {id} does not exist. Please check the provided course ID.");
        
        return course.Evls;
    }

    public void AddEvlToCourse(int courseId, int evlId)
    {
        var course = _context.Courses.Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException($"Course with ID {courseId} not found.");

        var evl = _context.Evls.FirstOrDefault(e => e.Id == evlId);

        if (evl == null)
            throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

        if (course.Evls.Any(e => e.Id == evlId))
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with Course ID {courseId}.");

        course.Evls.Add(evl);
        _context.SaveChanges();
    }
}