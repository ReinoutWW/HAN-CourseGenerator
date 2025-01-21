using HAN.Utilities.Messaging.Models;

namespace HAN.Services.Interfaces;

public class GradeSavedEventArgs : EventArgs
{
    public string MessageId { get; set; }
    public string PayloadJson { get; set; }
}

public class GradeRetrievedEventArgs : EventArgs
{
    public string MessageId { get; set; }
    public string PayloadJson { get; set; }
}

public class GetBlockResponseEventArgs : EventArgs
{
    public string MessageId { get; set; }
    public string PayloadJson { get; set; }
}

public class NodeMonitoringQueueEventArgs
{
    public string MessageId { get; set; }
    public string PayloadJson { get; set; }
}

public interface IResponseListener
{
    event EventHandler<GradeSavedEventArgs> GradeSavedReceived;
    event EventHandler<GradeRetrievedEventArgs> GradeRetrievedReceived;
    event EventHandler<NodeMonitoringQueueEventArgs> NodeMonitoringQueueReceived;
    event EventHandler<GetBlockResponseEventArgs> GetBlockResponseReceived;
}