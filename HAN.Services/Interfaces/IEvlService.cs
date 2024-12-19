using HAN.Services.DTOs;

namespace HAN.Services;

public interface IEvlService
{
    EvlDto CreateEvl(EvlDto evl);
    EvlDto GetEvlById(int id);
}