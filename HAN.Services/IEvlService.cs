using HAN.Domain.Entities;
using HAN.Services.DTOs;

namespace HAN.Services;

public interface IEvlService
{
    EvlResponseDto CreateEvl(CreateEvlDto evl);
    EvlResponseDto GetEvlById(int id);
}