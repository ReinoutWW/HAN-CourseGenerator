namespace HAN.Utilities.Messaging.Models;

public class GetBlockRequest
{
    public int BlockIndex { get; set; }
}

public class GetBlockResponse
{
    public int Index { get; set; }
    public long Timestamp { get; set; }
    public List<GetTransactionResponse> Transactions { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
}

public class GetTransactionResponse
{
    public string TransactionId { get; set; }
    public string Data { get; set; }
    public DateTime Timestamp { get; set; }

    public GetTransactionResponse(string transactionId, string data)
    {
        TransactionId = transactionId;
        Data = data;
        Timestamp = DateTime.UtcNow;
    }
}