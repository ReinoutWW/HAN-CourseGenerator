using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class EvlRepostory(AppDbContext context) : RepositoryBase(context), IEvlRepository
{
    public Evl CreateEvl(Evl evl)
    {
        if(evl == null) 
        {
            throw new ArgumentException($"Evl can not be null. User: {nameof(evl)}");
        }

        return Context.Evls.Add(evl).Entity;
    }

    public Evl GetEvlById(int evlId)
    {
        return Context.Evls.FirstOrDefault(p => p.Id == evlId) ?? throw new KeyNotFoundException();
    }
}