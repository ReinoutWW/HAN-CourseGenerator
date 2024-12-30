using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseRepository(AppDbContext context) : GenericRepository<Course>(context), ICourseRepository
{
    private readonly AppDbContext _context = context;

     public override void Update(Course entity)
    {
        var existingCourse = _context.Courses
            .Include(c => c.Schedule)
                .ThenInclude(s => s.ScheduleLines)
            .FirstOrDefault(c => c.Id == entity.Id);

        if (existingCourse == null)
            throw new InvalidOperationException($"Course with Id {entity.Id} not found.");

        _context.Entry(existingCourse).CurrentValues.SetValues(entity);
        existingCourse.EvlIds = entity.EvlIds;

        _context.SaveChanges();
    }

    public override void Add(Course entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.EvlIds = ValidateAndResolveEvlIds(entity.EvlIds);
        entity.Schedule ??= new Schedule();

        Entity.Add(entity);
        _context.SaveChanges();
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

    public override List<Course> GetAll()
    {
        return _context.Courses.ToList();
    }

    public IEnumerable<Evl> GetEvlsByCourseId(int id)
    {
        var evlIds = _context.Courses
            .Where(c => c.Id == id)
            .Select(c => c.EvlIds)
            .FirstOrDefault();

        if (evlIds == null)
            throw new KeyNotFoundException($"Course with id {id} does not exist.");

        return _context.Evls.Where(e => evlIds.Contains(e.Id)).ToList();
    }

    public void AddEvlToCourse(int courseId, int evlId)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException($"Course with ID {courseId} not found.");

        if (!course.EvlIds.Contains(evlId))
        {
            if (!_context.Evls.Any(e => e.Id == evlId))
                throw new KeyNotFoundException($"Evl with ID {evlId} not found.");

            course.EvlIds.Add(evlId);
            _context.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException($"Evl with ID {evlId} is already associated with Course ID {courseId}.");
        }
    }
}