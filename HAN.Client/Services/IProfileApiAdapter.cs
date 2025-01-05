using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public interface IProfileApiAdapter
{
    Task<Profile> GetUserProfile();
}