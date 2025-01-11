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

        _nodes.AddOrUpdate(status.NodeId, status, (key, oldVal) => status);
    }

    private void HandleRequestNodeList(IMessage message)
    {
        var nodeList = _nodes.Values.ToList(); 
        var responsePayload = System.Text.Json.JsonSerializer.Serialize(nodeList);

        var responseMessage = new GenericMessage
        {
            Id = message.Id,            
            Action = "NodeListResponse", 
            Payload = responsePayload
        };
        
        _publisher.Publish(responseMessage, "NodeListResponseQueue");
    }

    public static ConcurrentDictionary<string, NodeStatus> GetNodes() => _nodes;
}
