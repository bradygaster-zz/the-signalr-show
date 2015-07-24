using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Demo1_HitCounter
{
    [HubName("hitCounter")]
    public class HitCountHub : Hub
    {
        static int _hitCount = 0;

        // handle the connection

        // handle the disconnection
    }
}