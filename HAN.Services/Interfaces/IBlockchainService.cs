using HAN.Utilities.Messaging.Models;

namespace HAN.Services.Interfaces;

public interface IBlockchainService
{
    public Task<GetBlockResponse?> GetBlockAsync(int blockIndex); 
}