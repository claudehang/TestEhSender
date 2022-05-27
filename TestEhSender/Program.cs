using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestEhSender
{
    public class WeatherData
    {
        public int Temperature { get; set; }
        public int WindSpeed { get; set; }
        public Direction WindDirection { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Direction
        {
            North, South, East, West
        }

        public override string ToString()
        {
            return "{ Temp: " + Temperature.ToString() + " *C "
                 + "| Windspeed: " + WindSpeed.ToString() + " km/h "
                 + "| Wind Direction: " + WindDirection.ToString() + " }";
        }
    }

    class Program
    {
        // connection string to the Event Hubs namespace
        private const string connectionString = "Endpoint=sb://<EVENT HUB NAMESPACE>.servicebus.windows.net/;SharedAccessKeyName=<KEY NAME>;SharedAccessKey=<KEY>;EntityPath=<ENTITY>";

        // name of the event hub
        private const string eventHubName = "<EVENT HUB NAME>";

        // number of events to be sent to the event hub
        private const int numOfEvents = 30;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        static EventHubProducerClient producerClient;

        static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(connectionString, eventHubName);

            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            var rand = new Random();
            var windDirections = Enum.GetValues(typeof(WeatherData.Direction));

            for (int i = 1; i <= numOfEvents; i++)
            {
                var weatherData = new WeatherData
                {
                    Temperature = rand.Next(-20, 40),
                    WindSpeed = rand.Next(50, 600),
                    WindDirection = (WeatherData.Direction)windDirections.GetValue(rand.Next(windDirections.Length))
                };

                string weatherEvent = JsonConvert.SerializeObject(weatherData);

                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(weatherEvent))))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of {numOfEvents} events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
    }
}
