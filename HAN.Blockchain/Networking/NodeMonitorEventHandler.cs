using System.Collections.Concurrent;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

namespace HAN.Blockchain.Networking;

public class NodeMonitorEventHandler : IServiceMessageHandler<IMessage>
{
    private readonly IMessagePublisher _publisher;

    // We keep track of other nodes’ statuses
    private static ConcurrentDictionary<string, NodeStatus> _nodes =
        new ConcurrentDictionary<string, NodeStatus>();

    public NodeMonitorEventHandler(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public void Handle(IMessage message)
    {
        switch (message.Action)
        {
            case "NodeHeartbeat":
                HandleNodeHeartbeat(message);
                break;

            case "RequestNodeList":
                HandleRequestNodeList(message);
                break;

            default:
                // no-op or log
                break;
        }
    }

    private void HandleNodeHeartbeat(IMessage message)
    {
        var status = System.Text.Json.JsonSerializer.Deserialize<NodeStatus>(message.Payload);
        if (status == null) return;

        // Update or add the node status
        _nodes.AddOrUpdate(status.NodeId, status, (key, oldVal) => status);
        Console.WriteLine($"[NodeMonitor] Updated heartbeat: Node={status.NodeId}, Time={status.LastHeartbeat}");
    }

    private void HandleRequestNodeList(IMessage message)
    {
        Console.WriteLine("[NodeMonitor] Received RequestNodeList. Sending NodeListResponse...");

        // Get a snapshot of all known nodes
        var nodeList = _nodes.Values.ToList(); 
        
        // Serialize
        var responsePayload = System.Text.Json.JsonSerializer.Serialize(nodeList);

        // Create a response message
        // Use the *same* message.Id to let the requester correlate responses
        var responseMessage = new GenericMessage
        {
            Id = message.Id,             // Correlate with the original request
            Action = "NodeListResponse", // So the receiver can identify the response type
            Payload = responsePayload
        };

        // Publish to the queue used for node monitoring responses
        // Often this might be the same "NodeMonitoringQueue" or a dedicated queue
        _publisher.Publish(responseMessage, "NodeListResponseQueue");
    }

    // Expose a snapshot or accessor if needed
    public static ConcurrentDictionary<string, NodeStatus> GetNodes() => _nodes;
}
