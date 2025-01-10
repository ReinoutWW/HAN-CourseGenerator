using System.Security.Cryptography;
using System.Text;

namespace HAN.Blockchain.Models;

public class Block
{
    public int Index { get; set; }
    public long Timestamp { get; set; }
    public List<Transaction> Transactions { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }

    public Block(int index, List<Transaction> transactions, string previousHash)
    {
        Index = index;
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Transactions = transactions;
        PreviousHash = previousHash;
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        using var sha256 = SHA256.Create();
        var rawData = Index + Timestamp + PreviousHash + GetTransactionsAsString();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }

    private string GetTransactionsAsString()
    {
        var sb = new StringBuilder();
        foreach (var tx in Transactions)
        {
            sb.Append($"{tx.TransactionId}{tx.Data}{tx.Timestamp:o}");
        }
        return sb.ToString();
    }
}