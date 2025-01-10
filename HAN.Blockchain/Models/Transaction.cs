namespace HAN.Blockchain.Models;

public class Transaction
{
    public string TransactionId { get; set; }
    public string Data { get; set; }
    public DateTime Timestamp { get; set; }

    public Transaction(string transactionId, string data)
    {
        TransactionId = transactionId;
        Data = data;
        Timestamp = DateTime.UtcNow;
    }
}