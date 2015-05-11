using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace Demo5_Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080/"))
            {
                Console.WriteLine("Server running at http://localhost:8080/");
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }

    public class ValueHub : Hub
    {
        public void ChangeValue(int val)
        {
            Clients.All.valueChanged(val);
        }
    }
}
