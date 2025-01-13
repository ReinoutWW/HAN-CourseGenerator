using System.Collections.Concurrent;
using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

namespace HAN.Services;

public class MonitorService : IMonitorService
{
    private readonly IResponseListener _responseListener;

    // Track active node IDs and their last heartbeat time
    // (If you only need node IDs, a simple ConcurrentDictionary<string, bool> or a HashSet<string> would suffice)
    private static readonly ConcurrentDictionary<string, DateTime> _activeNodes 
        = new ConcurrentDictionary<string, DateTime>();

    // Expose an event or callback so the Blazor UI can react when the list of unique node IDs changes
    public event Action<int>? OnNodeCountUpdated;

    public MonitorService(IResponseListener responseListener)
    {
        _responseListener = responseListener;

        // Subscribe to the NodeMonitoringQueue event (instead of NodeListResponse)
        _responseListener.NodeMonitoringQueueReceived += NodeMonitoringQueueReceived;
    }

    /// <summary>
    /// Handles new heartbeat messages from the NodeMonitoringQueue.
    /// </summary>
    private void NodeMonitoringQueueReceived(object sender, NodeMonitoringQueueEventArgs e)
    {
        // Example: The event arg might contain NodeId (string) and possibly
        // additional information about the node status.
        var payload = System.Text.Json.JsonSerializer.Deserialize<GenericMessage>(e.PayloadJson) ?? throw new ArgumentException($"Invalid payload: {e.PayloadJson}");
        
        var nodeId = payload.NodeId; 
        var timeStamp = DateTime.UtcNow; 

        // Store/Update the node's last active time
        _activeNodes.AddOrUpdate(nodeId, timeStamp, (id, oldValue) => timeStamp);

        // If the Blazor UI needs the entire list, you can also pass it along
        // but for demonstration we only pass the count
        OnNodeCountUpdated?.Invoke(_activeNodes.Count);
    }

    public List<NodeStatus> GetActiveNodesAsync()
    {
        // Parse to list of NodeStatus objects
        var activeNodes = _activeNodes.Select(kvp => new NodeStatus
        {
            NodeId = kvp.Key,
            LastHeartbeat = kvp.Value
        }).ToList();
        
        return activeNodes;
    }
}