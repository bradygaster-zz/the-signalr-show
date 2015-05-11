using LittleBits.CloudBitApiClient;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo5_Theremin
{
    class Program
    {
        private static string ACCESS_TOKEN = ConfigurationManager.AppSettings["ACCESS_TOKEN"];
        private static string DEVICE_ID = ConfigurationManager.AppSettings["DEVICE_ID"];

        static void Main(string[] args)
        {
            Console.WriteLine("Theremin Client Running");

            var connection = new HubConnection("http://localhost:8080/");
            var hub = connection.CreateHubProxy("valueHub");
            var lastHit = DateTime.Now;

            hub.On<int>("valueChanged", (val) =>
            {
                val = 100 - val;

                if (DateTime.Now.Subtract(lastHit) > TimeSpan.FromMilliseconds(250))  {
                    Console.WriteLine("Sending {0} to synth", val);

                    Client
                        .Authenticate(ACCESS_TOKEN)
                        .SetOutput(new DeviceOutputRequest
                        {
                            Percent = (decimal)val,
                            DeviceId = DEVICE_ID,
                            DurationInMilliseconds = -1
                        });

                    Console.WriteLine("Sent {0} to synth", val);

                    lastHit = DateTime.Now;
                }
            });

            connection.Start().Wait();
            Console.ReadLine();
        }
    }
}
