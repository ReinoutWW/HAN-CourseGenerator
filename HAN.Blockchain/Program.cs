
// Welcome to the Event-driven blockchain storage node
// All grades for students will be saved using blockchain technology
// This will be a multi-node system, with each node storing a copy of the blockchain
// Using EDA, we will ensure that all nodes are in sync
// Also: There is a UI available to inspect all running nodes

using HAN.Blockchain.Models;
using HAN.Blockchain.Networking;
using HAN.Blockchain.Services;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;

Console.WriteLine("Node is up and running...");

// Each console instance can have a unique ID
var nodeId = args.Length > 0 ? args[0] : Guid.NewGuid().ToString().Substring(0, 8);
Console.Title = $"Blockchain Node [{nodeId}]";

// 1) Initialize in-memory blockchain
var localChain = new SimpleBlockchain();

// 2) Setup RabbitMQ or your chosen messaging system
IMessagePublisher publisher = new MessagePublisher("localhost");
var subscriber = new RabbitMqSubscriber(/* config */);

// 3) Create the handlers
var gradeService = new BlockchainGradeService(localChain, publisher);
var blockchainHandler = new BlockchainEventHandler(localChain, publisher);
var nodeMonitorHandler = new NodeMonitorEventHandler();

// 4) Subscribe to relevant queues
subscriber.Subscribe(gradeService, "SaveGradeQueue");
subscriber.Subscribe(gradeService, "GetGradeQueue");

// The queue that broadcasts new blocks or chain requests
subscriber.Subscribe(blockchainHandler, "BlockchainBlockBroadcastQueue");

// The queue that monitors nodes
subscriber.Subscribe(nodeMonitorHandler, "NodeMonitoringQueue");

// Optionally subscribe to "RequestNodeList" or other monitoring requests here

// 5) Start a heartbeat thread that publishes NodeHeartbeat
new Thread(() =>
{
    while (true)
    {
        var heartbeatMsg = new GenericMessage
        {
            Action = "NodeHeartbeat",
            Payload = System.Text.Json.JsonSerializer.Serialize(new NodeStatus
            {
                NodeId = nodeId,
                LastHeartbeat = DateTime.UtcNow
            })
        };
        publisher.Publish(heartbeatMsg, "NodeMonitoringQueue");
        Thread.Sleep(5000);
    }
})
{ IsBackground = true }.Start();

// 6) Keep the console app alive
Console.WriteLine($"Node [{nodeId}] started. Press ENTER to exit.");
Console.ReadLine();