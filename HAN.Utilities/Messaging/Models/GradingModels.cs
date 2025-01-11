namespace HAN.Utilities.Messaging.Models;

// Request/Response Models
public class SaveGradeRequest
{
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public string Grade { get; set; }
}

public class GradeSavedResponse
{
    public string TransactionId { get; set; }
    public int BlockIndex { get; set; }
    public string BlockHash { get; set; }
}

public class GetGradeRequest
{
    public string StudentId { get; set; }
}

public class GetGradeResponse
{
    public string StudentId { get; set; }
    public List<GradeRecord> Grades { get; set; }
}

public class GradeRecord
{
    public int BlockIndex { get; set; }
    public string BlockHash { get; set; }
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public string Grade { get; set; }
    public DateTime Timestamp { get; set; }
}

public class NodeStatus
{
    public string NodeId { get; set; }
    public DateTime LastHeartbeat { get; set; }
}