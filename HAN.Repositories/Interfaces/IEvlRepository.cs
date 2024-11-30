using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface IEvlRepository
{
    bool SaveChanges();
    
    Evl CreateEvl(Evl evl);
    Evl GetEvlById(int evlId);
}