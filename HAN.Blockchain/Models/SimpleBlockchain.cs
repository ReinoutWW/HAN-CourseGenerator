using System.Security.Cryptography;
using System.Text;

namespace HAN.Blockchain.Models;

public class SimpleBlockchain
{
    private readonly List<Block> _chain;
    private IReadOnlyList<Block> Chain => _chain;
    public const string GenesisBlockName = "GenesisBlock";

    public SimpleBlockchain()
    {
        _chain = new List<Block> { CreateGenesisBlock() };
    }
    
    public IReadOnlyList<Block> GetChain()
    {
        // Exclude the genesis block
        return Chain
            .Where(b => b.Transactions.All(t => t.TransactionId != GenesisBlockName))
            .ToList();
    }

    private Block CreateGenesisBlock()
    {
        var genesisTx = new Transaction(GenesisBlockName, "GENESIS_BLOCK");
        var genesisBlock = new Block(0, new List<Transaction> { genesisTx }, "0");
        return genesisBlock;
    }

    public Block GetLatestBlock()
    {
        return _chain[^1];
    }

    public Block AddBlock(List<Transaction> transactions)
    {
        var previousBlock = GetLatestBlock();
        var newBlock = new Block(previousBlock.Index + 1, transactions, previousBlock.Hash);
        _chain.Add(newBlock);
        return newBlock;
    }

    public bool IsValid()
    {
        for (int i = 1; i < _chain.Count; i++)
        {
            var currentBlock = _chain[i];
            var previousBlock = _chain[i - 1];

            // Recalculate the hash and compare
            if (currentBlock.Hash != currentBlock.CalculateHash())
                return false;

            if (currentBlock.PreviousHash != previousBlock.Hash)
                return false;
        }
        return true;
    }

    // Attempt to replace local chain with a "longer valid chain"
    public bool TryReplaceChain(List<Block> newChain)
    {
        // Basic checks: new chain must be longer and valid
        if (newChain.Count <= _chain.Count)
            return false;

        if (!IsChainValid(newChain))
            return false;

        // Accept the new chain
        _chain.Clear();
        _chain.AddRange(newChain);
        return true;
    }

    private bool IsChainValid(List<Block> chain)
    {
        for (int i = 1; i < chain.Count; i++)
        {
            var currentBlock = chain[i];
            var prevBlock = chain[i - 1];

            // Recalculate the hash
            using var sha256 = SHA256.Create();
            var rawData = currentBlock.Index + currentBlock.Timestamp
                + currentBlock.PreviousHash
                + GetTransactionsAsString(currentBlock.Transactions);

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var reCalcHash = Convert.ToBase64String(bytes);
            if (currentBlock.Hash != reCalcHash) return false;
            if (currentBlock.PreviousHash != prevBlock.Hash) return false;
        }
        return true;
    }

    private string GetTransactionsAsString(List<Transaction> transactions)
    {
        var sb = new StringBuilder();
        foreach (var tx in transactions)
        {
            sb.Append($"{tx.TransactionId}{tx.Data}{tx.Timestamp:o}");
        }
        return sb.ToString();
    }
}