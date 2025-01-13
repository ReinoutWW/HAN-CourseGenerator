using HAN.Utilities.Messaging.Models;

namespace HAN.Services.Interfaces;

public interface IMonitorService
{
    List<NodeStatus> GetActiveNodesAsync();
    public event Action<int>? OnNodeCountUpdated;
}