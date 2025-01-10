using HAN.Blockchain.Models;
using HAN.Utilities.Messaging.Abstractions;

namespace HAN.Blockchain.Services;

public class BlockchainGradeService : IServiceMessageHandler<IMessage>
{
    private readonly SimpleBlockchain _blockchain;
    private readonly IMessagePublisher _publisher;
    
    public BlockchainGradeService(SimpleBlockchain blockchain, IMessagePublisher publisher)
    {
        _blockchain = blockchain;
        _publisher = publisher;
    }

    public void Handle(IMessage message)
    {
        switch (message.Action)
        {
            case "SaveGrade":
                HandleSaveGrade(message);
                break;
            case "GetGrade":
                HandleGetGrade(message);
                break;
            default:
                Console.WriteLine($"[GradeService] Unrecognized action: {message.Action}");
                break;
        }
    }

    private void HandleSaveGrade(IMessage message)
    {
        // e.g. { "StudentId": "S123", "CourseId": "C101", "Grade": "A" }
        var request = System.Text.Json.JsonSerializer.Deserialize<SaveGradeRequest>(message.Payload);
        var tx = new Transaction(Guid.NewGuid().ToString(), message.Payload);

        // Create a new block (in a real system, you might batch multiple transactions)
        var newBlock = _blockchain.AddBlock(new List<Transaction> { tx });
        Console.WriteLine($"[GradeService] New block created. Index={newBlock.Index}, Hash={newBlock.Hash}");

        // Broadcast the block to other nodes (via RabbitMQ)
        var blockPayload = System.Text.Json.JsonSerializer.Serialize(newBlock);
        var blockMessage = new GenericMessage()
        {
            Action = "NewBlockCreated",
            Payload = blockPayload
        };
        _publisher.Publish(blockMessage, "BlockchainBlockBroadcastQueue");

        // Optionally respond with a "GradeSaved" event
        var responsePayload = System.Text.Json.JsonSerializer.Serialize(new GradeSavedResponse
        {
            TransactionId = tx.TransactionId,
            BlockIndex = newBlock.Index,
            BlockHash = newBlock.Hash
        });
        var responseMessage = new GenericMessage
        {
            Action = "GradeSaved",
            Payload = responsePayload
        };
        _publisher.Publish(responseMessage, "GradeSavedQueue");
    }

    private void HandleGetGrade(IMessage message)
    {
        var request = System.Text.Json.JsonSerializer.Deserialize<GetGradeRequest>(message.Payload);
        var matchingRecords = new List<GradeRecord>();

        foreach (var block in _blockchain.Chain)
        {
            foreach (var tx in block.Transactions)
            {
                var saveGradeReq = System.Text.Json.JsonSerializer.Deserialize<SaveGradeRequest>(tx.Data);
                if (saveGradeReq.StudentId == request.StudentId)
                {
                    matchingRecords.Add(new GradeRecord
                    {
                        BlockIndex = block.Index,
                        BlockHash = block.Hash,
                        CourseId = saveGradeReq.CourseId,
                        Grade = saveGradeReq.Grade,
                        Timestamp = tx.Timestamp
                    });
                }
            }
        }

        var response = new GetGradeResponse
        {
            StudentId = request.StudentId,
            Grades = matchingRecords
        };

        var responseMessage = new GenericMessage
        {
            Action = "GradeRetrieved",
            Payload = System.Text.Json.JsonSerializer.Serialize(response)
        };
        // Publish to a queue or respond directly
        _publisher.Publish(responseMessage, "GradeRetrievedQueue");
    }
}

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
    public string CourseId { get; set; }
    public string Grade { get; set; }
    public DateTime Timestamp { get; set; }
}