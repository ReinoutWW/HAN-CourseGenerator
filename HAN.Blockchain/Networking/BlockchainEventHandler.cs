using HAN.Blockchain.Models;
using HAN.Utilities.Messaging.Abstractions;

namespace HAN.Blockchain.Networking;

 public class BlockchainEventHandler : IServiceMessageHandler<IMessage>
    {
        private readonly SimpleBlockchain _blockchain;
        private readonly IMessagePublisher _publisher;

        public BlockchainEventHandler(SimpleBlockchain blockchain, IMessagePublisher publisher)
        {
            _blockchain = blockchain;
            _publisher = publisher;
        }

        public void Handle(IMessage message)
        {
            switch (message.Action)
            {
                case "NewBlockCreated":
                    HandleNewBlock(message);
                    break;
                case "RequestFullChain":
                    HandleRequestFullChain(message);
                    break;
                case "FullChainResponse":
                    HandleFullChainResponse(message);
                    break;
                default:
                    // no-op
                    break;
            }
        }

        private void HandleNewBlock(IMessage message)
        {
            var incomingBlock = System.Text.Json.JsonSerializer.Deserialize<Block>(message.Payload);
            Console.WriteLine($"[BlockchainEventHandler] Received block index={incomingBlock.Index}, hash={incomingBlock.Hash}");

            // 1) Check if this block can just be appended
            var latestBlock = _blockchain.GetLatestBlock();
            if (incomingBlock.Index == latestBlock.Index + 1 &&
                incomingBlock.PreviousHash == latestBlock.Hash)
            {
                // Just append the block
                // We can’t directly append to the chain list. We need the AddBlock logic to keep consistency:
                _blockchain.AddBlock(incomingBlock.Transactions);
                Console.WriteLine("[BlockchainEventHandler] Appended the new block to local chain");
            }
            else
            {
                // 2) If indexes are off, we might need to request the full chain from the sender
                Console.WriteLine("[BlockchainEventHandler] Block index mismatch or chain behind; requesting full chain...");

                var requestChainMsg = new GenericMessage
                {
                    Action = "RequestFullChain",
                    Payload = "" // or include node info
                };
                // Publish to a “broadcast” or specifically route to the node that sent it
                // For simplicity, we do a broadcast
                _publisher.Publish(requestChainMsg, "BlockchainBlockBroadcastQueue");
            }
        }

        private void HandleRequestFullChain(IMessage message)
        {
            // Another node wants our full chain
            var chainPayload = System.Text.Json.JsonSerializer.Serialize(_blockchain.GetChain());
            var responseMsg = new GenericMessage
            {
                Action = "FullChainResponse",
                Payload = chainPayload
            };
            _publisher.Publish(responseMsg, "BlockchainBlockBroadcastQueue");
        }

        private void HandleFullChainResponse(IMessage message)
        {
            var incomingChain = System.Text.Json.JsonSerializer.Deserialize<List<Block>>(message.Payload);
            Console.WriteLine($"[BlockchainEventHandler] Received full chain from peer. Length={incomingChain.Count}");

            // Attempt to replace local chain if incoming one is longer & valid
            if (_blockchain.TryReplaceChain(incomingChain))
            {
                Console.WriteLine("[BlockchainEventHandler] Local chain replaced with a longer valid chain");
            }
            else
            {
                Console.WriteLine("[BlockchainEventHandler] Ignored incoming chain (not longer or invalid)");
            }
        }
    }