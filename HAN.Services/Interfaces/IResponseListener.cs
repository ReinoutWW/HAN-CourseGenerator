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

public class NodeListResponseEventArgs : EventArgs
{
    public string MessageId { get; set; }
    public string PayloadJson { get; set; }
}
public interface IResponseListener
{
    event EventHandler<GradeSavedEventArgs> GradeSavedReceived;
    event EventHandler<GradeRetrievedEventArgs> GradeRetrievedReceived;
    event EventHandler<NodeListResponseEventArgs> NodeListResponseReceived;
}