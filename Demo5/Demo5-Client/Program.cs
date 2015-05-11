using Microsoft.AspNet.SignalR.Client;
using System;

namespace Demo5_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnection("http://localhost:8080/");
            var hub = connection.CreateHubProxy("valueHub");
            hub.On("valueChanged", (g) => Console.WriteLine(g));
            connection.Start().Wait();
            Console.ReadLine();
        }
    }
}
