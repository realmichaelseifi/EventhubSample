using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;


namespace EventhubSampleWriter
{
    class Program
    {
        //private const string connectionString = "<event_hub_connection_string>";
        //private const string eventHub = "<event_hub_name>";

        // GETTING CONFIGURATIONS FROM A STATIC CONFIG FILE
        private static string connectionString = Config.ConnectionString;
        private static string eventHub = Config.EventHubName;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await using (var producerCLient = new EventHubProducerClient(connectionString: connectionString, eventHubName: eventHub))
            {
                // CREATE A BATCH OF EVENTS
                using EventDataBatch eventDataBatch = await producerCLient.CreateBatchAsync();

                // ADD SOME EVNETS TO THE BATCH
                eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("my first event")));
                eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("my second event")));
                eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("my third event")));
                eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("my fourth event")));

                // SEND THE BATCH TO THE HUB
                await producerCLient.SendAsync(eventDataBatch);


                // WRITE TO LOG
                Console.WriteLine( $"{eventDataBatch.Count} events have been sent to the hub!");

            }
        }
    }
}
