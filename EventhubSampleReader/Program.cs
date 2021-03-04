using System;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using System.Threading.Tasks;
using System.Text;

namespace EventhubSampleReader
{
    class Program
    {

        //private const string connectionString = "<event_hub_connection_string>";
        //private const string eventHub = "<event_hub_name>";
        //private const string blobStorageConnectionString = "<blob_connection_string>";
        //private const string blobContainerName = "<blobCOntainerName>";


        // getting configurations from a static config file
        private static string connectionString = Config.ConnectionString;
        private static string eventHub = Config.EventHubName;
        private static string blobStorageConnectionString = Config.BLobStorageConnectionString;
        private static string blobContainerName = Config.BlobContainerName;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // CHOOSE THE DEFAULT CONSUMER. OTHERWISE CREATE A NEW ONE IN PORTAL AND SELECT IT HERE
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // CREATE BLOB CONTAINER
            BlobContainerClient blobContainerClient = 
                new BlobContainerClient(blobStorageConnectionString, blobContainerName);

            // CREATE EVENT PROCESSOR
            EventProcessorClient eventProcessorClient = 
                new EventProcessorClient(blobContainerClient, consumerGroup, connectionString, eventHub);

            // REGISTER HANDLERS
            eventProcessorClient.ProcessEventAsync += EventProcessorClient_ProcessEventAsync;
            eventProcessorClient.ProcessErrorAsync += EventProcessorClient_ProcessErrorAsync;

            Console.WriteLine("Starting the process!");
            // START THE PROCESS
            await eventProcessorClient.StartProcessingAsync().ConfigureAwait(false);

            Console.WriteLine("Pause for 10 seconds!");
            // PAUSE FOR TEN SECONDS
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);

            Console.WriteLine("Stopping the process!");
            // STOP THE PROCESS
            await eventProcessorClient.StopProcessingAsync().ConfigureAwait(false);

            // EXIT
            Console.WriteLine("hit any key to exit!");

            Environment.Exit(1);

        }

        private static Task EventProcessorClient_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            // WRITE TO LOG
            Console.WriteLine($"\tPartition {arg.PartitionId} : Error");
            Console.WriteLine($"{arg.Exception.Message}");

            return Task.CompletedTask;
        }

        private static async Task EventProcessorClient_ProcessEventAsync(ProcessEventArgs arg)
        {
            // WRITE TO LOG
            Console.WriteLine($"\tEvent {Encoding.UTF8.GetString(arg.Data.EventBody.ToArray())}");

            // UPDATE CHECKPOINT
            await arg.UpdateCheckpointAsync(arg.CancellationToken);

        }
    }
}
