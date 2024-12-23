using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class EvlService(IEvlRepository evlRepository, IMapper mapper, IValidationService validationService) : IEvlService
{
    public EvlDto CreateEvl(EvlDto evl)
    {
        validationService.Validate(evl);
        
        var evlEntity = mapper.Map<Evl>(evl);
        evlRepository.Add(evlEntity);
        
        return mapper.Map<EvlDto>(evlEntity);
    }

    public EvlDto GetEvlById(int id)
    {
        var evl = evlRepository.GetById(id);
        return mapper.Map<EvlDto>(evl);
    }

    public IEnumerable<EvlDto> GetAllEvls()
    {
        var evls = evlRepository.GetAll();
        return mapper.Map<IEnumerable<EvlDto>>(evls);
    }
}