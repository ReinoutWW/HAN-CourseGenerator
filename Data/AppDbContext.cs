using System.ComponentModel.DataAnnotations;
using HAN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HAN.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    
    public override int SaveChanges()
    {
        ValidateEntities();
        return base.SaveChanges();
    }
    
    private void ValidateEntities()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e.Entity);

        foreach (var entity in entities)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);

            if (!Validator.TryValidateObject(entity, context, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    throw new ValidationException(validationResult.ErrorMessage);
                }
            }
        }
    }
}