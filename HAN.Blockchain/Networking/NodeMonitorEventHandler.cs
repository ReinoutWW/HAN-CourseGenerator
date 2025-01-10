using System.Collections.Concurrent;
using HAN.Utilities.Messaging.Abstractions;

namespace HAN.Blockchain.Networking;

public class NodeStatus
{
    public string NodeId { get; set; }
    public DateTime LastHeartbeat { get; set; }
}

public class NodeMonitorEventHandler : IServiceMessageHandler<IMessage>
{
    // We keep track of other nodes’ statuses
    private static ConcurrentDictionary<string, NodeStatus> _nodes = new ConcurrentDictionary<string, NodeStatus>();

    public void Handle(IMessage message)
    {
        if (message.Action == "NodeHeartbeat")
        {
            var status = System.Text.Json.JsonSerializer.Deserialize<NodeStatus>(message.Payload);
            _nodes.AddOrUpdate(status.NodeId, status, (key, oldVal) => status);
        }
        else if (message.Action == "RequestNodeList")
        {
            // We could respond with the list of known nodes
            // But this might be better served by an API call if you have a front-end
        }
    }

    public static ConcurrentDictionary<string, NodeStatus> GetNodes() => _nodes;
}