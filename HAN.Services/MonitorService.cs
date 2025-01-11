using System.Collections.Concurrent;
using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

namespace HAN.Services;

public class MonitorService : IMonitorService
{
    private readonly IMessagePublisher _publisher;
    private readonly IResponseListener _responseListener;

    private static ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests
        = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

    public MonitorService(IMessagePublisher publisher, IResponseListener responseListener)
    {
        _publisher = publisher;
        _responseListener = responseListener;

        // Subscribe to the NodeListResponse event
        _responseListener.NodeListResponseReceived += OnNodeListResponseReceived;
    }

    public async Task<List<NodeStatus>> GetActiveNodesAsync()
    {
        // 1) Construct the request message
        var message = new GenericMessage
        {
            Id = Guid.NewGuid().ToString(),
            Action = "RequestNodeList",
            Payload = "" // Might be empty or have some additional data
        };

        // 2) Create TCS
        var tcs = new TaskCompletionSource<string>();
        _pendingRequests[message.Id] = tcs;

        // 3) Publish (could be to "NodeMonitoringQueue" or a dedicated queue)
        _publisher.Publish(message, "NodeMonitoringQueue");

        // 4) Wait for response or time out
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        using (cts)
        {
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(-1, cts.Token));
            if (completedTask != tcs.Task)
            {
                _pendingRequests.TryRemove(message.Id, out _);
                throw new TimeoutException("No node list response in time.");
            }
        }

        // 5) Parse the JSON result
        var json = await tcs.Task;
        var nodeList = System.Text.Json.JsonSerializer.Deserialize<List<NodeStatus>>(json);
        return nodeList;
    }

    private void OnNodeListResponseReceived(object sender, NodeListResponseEventArgs e)
    {
        if (_pendingRequests.TryRemove(e.MessageId, out var tcs))
        {
            tcs.SetResult(e.PayloadJson);
        }
    }
}