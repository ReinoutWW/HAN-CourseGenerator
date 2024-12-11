using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public interface IEvlService
{
    EvlResponseDto CreateEvl(CreateEvlDto evl);
    EvlResponseDto GetEvlById(int id);
}

public class EvlService(IEvlRepository evlRepostory, IMapper mapper, IValidationService validationService) : IEvlService
{
    public EvlResponseDto CreateEvl(CreateEvlDto evl)
    {
        var evlDomainEntity = mapper.Map<Evl>(evl);
        
        validationService.Validate(evlDomainEntity);
        
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