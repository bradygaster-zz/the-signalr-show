using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartup(typeof(ScalingWithRedis.Startup))]

namespace ScalingWithRedis
{
    public class Startup
    {
        void SetupScaleOut()
        {
            GlobalHost.DependencyResolver.UseRedis("YOUR_AZURE_EDIS.redis.cache.windows.net",
                6379,
                "YOUR REDIS KEY",
                "WhateverYouWant");
        }

        public void Configuration(IAppBuilder app)
        {
            SetupScaleOut();

            app.MapSignalR();
        }
    }
}
