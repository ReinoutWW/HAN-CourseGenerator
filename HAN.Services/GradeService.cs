using System.Collections.Concurrent;
using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

namespace HAN.Services;

public class GradeService : IGradeService
{
    private readonly IMessagePublisher _publisher;
    private readonly IResponseListener _responseListener;

    // We might store pending tasks using a correlation ID for each request
    private static ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests
        = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

    public GradeService(IMessagePublisher publisher, IResponseListener responseListener)
    {
        _publisher = publisher;
        _responseListener = responseListener;

        // Subscribe to relevant "GradeSaved" / "GradeRetrieved" events
        // so we can complete pending tasks.
        _responseListener.GradeSavedReceived += OnGradeSavedReceived;
        _responseListener.GradeRetrievedReceived += OnGradeRetrievedReceived;
    }

    public async Task SaveGradeAsync(string studentId, string courseId, string grade)
    {
        // 1) Construct the request
        var requestPayload = new SaveGradeRequest
        {
            StudentId = studentId,
            CourseId = courseId,
            Grade = grade
        };
        var message = new GenericMessage
        {
            Id = Guid.NewGuid().ToString(), // correlation ID
            Action = "SaveGrade",
            Payload = System.Text.Json.JsonSerializer.Serialize(requestPayload)
        };

        // 2) Create a TaskCompletionSource that we’ll complete when we get "GradeSaved"
        var tcs = new TaskCompletionSource<string>();
        _pendingRequests[message.Id] = tcs;

        // 3) Publish the event
        _publisher.Publish(message, "SaveGradeQueue");

        // 4) Wait for the "GradeSaved" event (via OnGradeSavedReceived) or timeout
        // This ensures the Blazor caller won't proceed until the blockchain confirms
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15)); // example timeout
        using (cts)
        {
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(-1, cts.Token));
            if (completedTask != tcs.Task)
            {
                // Timed out
                _pendingRequests.TryRemove(message.Id, out _);
                throw new TimeoutException("Did not receive GradeSaved response in time.");
            }
        }
    }

    public async Task<List<GradeRecord>> GetGradesAsync(string studentId)
    {
        // 1) Construct the request
        var requestPayload = new GetGradeRequest { StudentId = studentId };
        var message = new GenericMessage
        {
            Id = Guid.NewGuid().ToString(),
            Action = "GetGrade",
            Payload = System.Text.Json.JsonSerializer.Serialize(requestPayload)
        };

        // 2) Create a TCS for the "GradeRetrieved" event
        var tcs = new TaskCompletionSource<string>();
        _pendingRequests[message.Id] = tcs;

        // 3) Publish
        _publisher.Publish(message, "GetGradeQueue");

        // 4) Wait for the response or timeout
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        using (cts)
        {
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(-1, cts.Token));
            if (completedTask != tcs.Task)
            {
                _pendingRequests.TryRemove(message.Id, out _);
                throw new TimeoutException("Did not receive GradeRetrieved response in time.");
            }
        }

        // 5) Parse the result
        var json = await tcs.Task; // the GradeRetrieved payload
        var gradeResponse = System.Text.Json.JsonSerializer.Deserialize<GetGradeResponse>(json);
        return gradeResponse.Grades;
    }

    private void OnGradeSavedReceived(object sender, GradeSavedEventArgs e)
    {
        // e.MessageId -> correlation ID
        if (_pendingRequests.TryRemove(e.MessageId, out var tcs))
        {
            // Complete the TCS with some success indicator if needed
            tcs.SetResult("OK");
        }
    }

    private void OnGradeRetrievedReceived(object sender, GradeRetrievedEventArgs e)
    {
        if (_pendingRequests.TryRemove(e.MessageId, out var tcs))
        {
            // The event args contain the JSON payload
            tcs.SetResult(e.PayloadJson);
        }
    }
}
