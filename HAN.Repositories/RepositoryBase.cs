using HAN.Data;

namespace HAN.Repositories;

public class RepositoryBase(AppDbContext context)
{
    protected readonly AppDbContext Context = context;

    public bool SaveChanges()
    {
        return Context.SaveChanges() >= 0;
    }
}