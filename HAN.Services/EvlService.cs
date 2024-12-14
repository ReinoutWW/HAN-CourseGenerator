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

public class EvlService(IEvlRepository evlRepository, IMapper mapper, IValidationService validationService) : IEvlService
{
    public EvlResponseDto CreateEvl(CreateEvlDto evl)
    {
        var evlEntity = mapper.Map<Evl>(evl);
        
        validationService.Validate(evlEntity);
        evlRepository.Add(evlEntity);
        
        return mapper.Map<EvlResponseDto>(evlEntity);
    }

    public EvlResponseDto GetEvlById(int id)
    {
        var evl = evlRepository.GetById(id);
        return mapper.Map<EvlResponseDto>(evl);
    }
}