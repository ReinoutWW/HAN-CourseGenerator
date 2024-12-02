using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;

namespace HAN.Services;

public interface IEvlService
{
    EvlResponseDto CreateEvl(CreateEvlDto evl);
    EvlResponseDto GetEvlById(int id);
}

public class EvlService(IEvlRepository evlRepostory, IMapper mapper) : IEvlService
{
    [ValidateEntities]
    public EvlResponseDto CreateEvl(CreateEvlDto evl)
    {
        var evlResult = evlRepostory.CreateEvl(
            mapper.Map<Data.Entities.Evl>(evl)
        );
        evlRepostory.SaveChanges();
        
        return mapper.Map<EvlResponseDto>(evlResult);
    }

    public EvlResponseDto GetEvlById(int id)
    {
        var evl = evlRepostory.GetEvlById(id);
        return mapper.Map<EvlResponseDto>(evl);
    }
}