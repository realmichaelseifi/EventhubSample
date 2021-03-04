using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;


namespace EventhubSampleWriter
{
    class Program
    {
        private const string connectionString = "<event_hub_connection_string>";
        private const string eventHub = "<event_hub_name>";


        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await using (var producerCLient = new EventHubProducerClient(connectionString: connectionString, eventHubName: eventHub))
            {
                // create a batch of events
                using EventDataBatch eventDataBatch = await producerCLient.CreateBatchAsync();


            }
        }
    }
}
