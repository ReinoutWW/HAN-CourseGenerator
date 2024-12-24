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
        var existingCourse = Context.Set<Course>()
            .Include(c => c.Evls)
            .FirstOrDefault(c => c.Id == entity.Id);

        if (existingCourse == null)
            throw new InvalidOperationException($"Course with Id {entity.Id} not found.");

        Context.Entry(existingCourse).CurrentValues.SetValues(entity);
        SynchronizeCollection(existingCourse.Evls, entity.Evls);

        Context.SaveChanges();
    }

    public override void Add(Course entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.Evls = ValidateAndResolveEvls(entity.Evls);

        Entity.Add(entity);
        Context.SaveChanges();
    }

    private List<Evl> ValidateAndResolveEvls(List<Evl> evls)
    {
        if (evls == null)
            throw new ArgumentNullException(nameof(evls));

        var resolvedEvls = new List<Evl>();
        foreach (var evl in evls)
        {
            resolvedEvls.Add(GetExistingEvlOrThrow(evl));
        }

        return resolvedEvls;
    }

    private Evl GetExistingEvlOrThrow(Evl evl)
    {
        if (evl.Id <= 0)
            throw new InvalidOperationException("Cannot add a new Evl in this method. Only existing Evl references are allowed.");

        var existingEvl = Context.Set<Evl>().Find(evl.Id);
        if (existingEvl == null)
            throw new InvalidOperationException($"Evl with Id {evl.Id} not found.");

        return existingEvl;
    }

    public override List<Course> GetAll()
    {
        return _context.Courses
            .Include(c => c.Evls)
            .ToList();
    }

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