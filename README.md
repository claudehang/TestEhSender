# C# program to send JSON data to Azure Event Hub

Steps:
1. Clone the project and replace the private members `connectionString` and `eventHubName` of `Program` class in `Program.cs` file.
2. Build and run the VS solution.
3. Go to Azure Portal -> Your Event Hub Namespace -> Your Event Hub -> Process data -> Enable real time insights from events.
4. You should see the sample data in your Event Hub like the screenshot below.

![Eh_Query_Result](https://user-images.githubusercontent.com/13774165/170662120-0a8fc712-443e-43a1-a917-38c3be9f3de6.jpg)

Reference:
1. https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send
2. https://www.c-sharpcorner.com/article/sending-event-data-to-azure-event-hub
