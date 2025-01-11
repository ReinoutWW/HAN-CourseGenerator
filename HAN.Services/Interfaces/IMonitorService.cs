using HAN.Utilities.Messaging.Models;

namespace HAN.Services.Interfaces;

public interface IMonitorService
{
    Task<List<NodeStatus>> GetActiveNodesAsync();
}