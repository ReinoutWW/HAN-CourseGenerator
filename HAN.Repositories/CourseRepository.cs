using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class CourseRepository(AppDbContext context) : GenericRepository<Course>(context), ICourseRepository
{
    private readonly AppDbContext _context = context;
    
    public override void Add(Course entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        // We'll iterate over each Evl in entity.Evls
        for (int i = 0; i < entity.Evls.Count; i++)
        {
            var inputEvl = entity.Evls[i];

            // Must have a valid ID to be considered existing
            if (inputEvl.Id <= 0)
            {
                throw new InvalidOperationException(
                    "Cannot add a new Evl in this method. Only existing Evl references are allowed."
                );
            }

            // STEP 1: Check if the context is already tracking an Evl with the same Id
            var trackedEvl = _context.ChangeTracker.Entries<Evl>()
                .Where(e => e.Entity.Id == inputEvl.Id)
                .Select(e => e.Entity)
                .FirstOrDefault();

            if (trackedEvl != null)
            {
                // We already have an Evl with the same Id in memory, so reuse it.
                entity.Evls[i] = trackedEvl; 
                // No need to Attach since it's already tracked.
            }
            else
            {
                // STEP 2: Not tracked yet, fetch from DB
                var existingEvl = _context.Set<Evl>().Find(inputEvl.Id);
                if (existingEvl == null)
                {
                    throw new InvalidOperationException(
                        $"Evl with ID={inputEvl.Id} does not exist in the database."
                    );
                }

                // Attach it to ensure EF sees it as an existing entity
                _context.Set<Evl>().Attach(existingEvl);

                // Now replace the Evl reference in the list
                entity.Evls[i] = existingEvl;
            }
        }

        // Finally, add the main entity
        Entity.Add(entity);

        // Save once at the end
        _context.SaveChanges();
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