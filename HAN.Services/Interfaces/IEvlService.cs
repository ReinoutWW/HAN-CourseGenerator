using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IEvlService
{
    EvlDto CreateEvl(EvlDto evl);
    EvlDto GetEvlById(int id);
    IEnumerable<EvlDto> GetAllEvls();
}