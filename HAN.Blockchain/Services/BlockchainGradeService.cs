using HAN.Blockchain.Models;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.Models;

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
            case "GetBlock":
                HandleGetBlock(message);
                break;
            default:
                Console.WriteLine($"[GradeService] Unrecognized action: {message.Action}");
                break;
        }
    }

    private void HandleGetBlock(IMessage message)
    {
        // e.g. { "Index": 3 }
        var request = System.Text.Json.JsonSerializer.Deserialize<GetBlockRequest>(message.Payload);
        var block = _blockchain.GetBlock(request.BlockIndex);
        
        var responsePayload = System.Text.Json.JsonSerializer.Serialize(block);
        var responseMessage = new GenericMessage
        {
            Id = message.Id,
            Action = "BlockRetrieved",
            Payload = responsePayload
        };
        
        _publisher.Publish(responseMessage, "BlockRetrievedQueue");
        Console.WriteLine("[GradeService] Queue 'BlockRetrievedQueue' event published");
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
            Id = message.Id,
            Action = "GradeSaved",
            Payload = responsePayload
        };
        _publisher.Publish(responseMessage, "GradeSavedQueue");
        Console.WriteLine("[GradeService] Queue 'GradeSavedQueue' event published");
    }

    private void HandleGetGrade(IMessage message)
    {
        var request = System.Text.Json.JsonSerializer.Deserialize<GetGradeRequest>(message.Payload);
        var matchingRecords = new List<GradeRecord>();

        foreach (var block in _blockchain.GetChain())
        {
            foreach (var tx in block.Transactions)
            {
                var saveGradeReq = System.Text.Json.JsonSerializer.Deserialize<SaveGradeRequest>(tx.Data);
                if (saveGradeReq.StudentId == request.StudentId || string.IsNullOrEmpty(request.StudentId))
                {
                    matchingRecords.Add(new GradeRecord
                    {
                        StudentId = saveGradeReq.StudentId,
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
            Id = message.Id,
            Action = "GradeRetrieved",
            Payload = System.Text.Json.JsonSerializer.Serialize(response)
        };
        // Publish to a queue or respond directly
        _publisher.Publish(responseMessage, "GradeRetrievedQueue");
        Console.WriteLine("[GradeService] Queue 'GradeRetrievedQueue' event published");
    }
}