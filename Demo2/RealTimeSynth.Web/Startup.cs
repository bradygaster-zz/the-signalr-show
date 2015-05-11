using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using LittleBits.CloudBitApiClient;

[assembly: OwinStartup(typeof(RealTimeSynth.Web.Startup))]

namespace RealTimeSynth.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
