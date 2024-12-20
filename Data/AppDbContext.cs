using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;
using File = HAN.Data.Entities.File;

namespace HAN.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Evl> Evls { get; set; }
    public DbSet<CourseComponent> CourseComponents { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ScheduleLine> ScheduleLines { get; set; }
    
    public DbSet<ExampleEntity> GenericTests { get; set; }

    public override int SaveChanges()
    {
        ValidateEntities();
        return base.SaveChanges();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .Property(c => c.CreatedAt)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<CourseComponent>()
            .HasDiscriminator<string>("ComponentType")
            .HasValue<CourseComponent>("Base")
            .HasValue<Lesson>("Lesson")
            .HasValue<Exam>("Exam");
        
        modelBuilder.Entity<Schedule>()
            .HasMany(s => s.ScheduleLines)
            .WithOne()
            .HasForeignKey(sl => sl.Id) 
            .OnDelete(DeleteBehavior.Cascade); 
    }
    
    private void ValidateEntities()
    {
        var entities = GetChangedEntities();
        var validationExceptions = EntityValidator.GetValidationExceptionsForEntities(entities);

        if (validationExceptions.Count != 0)
            throw new AggregateException("Validation failed for one or more entities.", validationExceptions);
    }

    private IEnumerable<object> GetChangedEntities()
    {
        return ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity);
    }
}