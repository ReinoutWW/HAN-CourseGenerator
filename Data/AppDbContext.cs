﻿using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;
using File = HAN.Data.Entities.File;

namespace HAN.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ScheduleLine> ScheduleLines { get; set; }
    public DbSet<Evl> Evls { get; set; }
    public DbSet<CourseComponent> CourseComponents { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ExampleEntity> GenericTests { get; set; }

    public override int SaveChanges()
    {
        ValidateEntities();
        return base.SaveChanges();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Schedule)
            .WithOne(s => s.Course)
            .HasForeignKey<Schedule>(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade); // Enforce cascading delete
        
        modelBuilder.Entity<Schedule>()
            .HasMany(s => s.ScheduleLines)
            .WithOne(sl => sl.Schedule)
            .HasForeignKey(sl => sl.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CourseComponent>()
            .HasDiscriminator<string>("ComponentType")
            .HasValue<CourseComponent>("Base")
            .HasValue<Lesson>("Lesson")
            .HasValue<Exam>("Exam");
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