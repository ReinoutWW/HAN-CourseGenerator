using System.Collections.Concurrent;
using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

namespace HAN.Services;

public class BlockchainService : IBlockchainService
{
    private readonly IMessagePublisher _publisher;
    private readonly IResponseListener _responseListener;

    // We might store pending tasks using a correlation ID for each request
    private static ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests
        = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

    public BlockchainService(IMessagePublisher publisher, IResponseListener responseListener)
    {
        _publisher = publisher;
        _responseListener = responseListener;

        // Subscribe to relevant "GradeSaved" / "GradeRetrieved" events
        // so we can complete pending tasks.
        _responseListener.GetBlockResponseReceived += OnBlockResponseReceived;
    }
    
    private void OnBlockResponseReceived(object sender, GetBlockResponseEventArgs e)
    {
        if (_pendingRequests.TryRemove(e.MessageId, out var tcs))
        {
            // The event args contain the JSON payload
            tcs.SetResult(e.PayloadJson);
        }
    }
    
    public async Task<GetBlockResponse> GetBlockAsync(int blockIndex)
    {
        // 1) Construct the request
        var requestPayload = new GetBlockRequest { BlockIndex = blockIndex };
        var message = new GenericMessage
        {
            Id = Guid.NewGuid().ToString(),
            Action = "GetBlock",
            Payload = System.Text.Json.JsonSerializer.Serialize(requestPayload)
        };

        // 2) Create a TCS for the "GradeRetrieved" event
        var tcs = new TaskCompletionSource<string>();
        _pendingRequests[message.Id] = tcs;

        // 3) Publish
        _publisher.Publish(message, "GetBlockQueue");

        // 4) Wait for the response or timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        using (cts)
        {
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(-1, cts.Token));
            if (completedTask != tcs.Task)
            {
                _pendingRequests.TryRemove(message.Id, out _);
                throw new TimeoutException("Did not receive BlockRetrievedQueue response in time.");
            }
        }

        // 5) Parse the result
        var json = await tcs.Task; // the GradeRetrieved payload
        var blockResponse = System.Text.Json.JsonSerializer.Deserialize<GetBlockResponse>(json);
        return blockResponse ?? throw new Exception("Failed to parse GetBlockResponse");
    }
}