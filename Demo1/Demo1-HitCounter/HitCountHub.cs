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

        public override Task OnConnected()
        {
            _hitCount += 1;

            Clients.All.hitCounted(_hitCount);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _hitCount -= 1;
            Clients.All.hitCounted(_hitCount);

            return base.OnDisconnected(stopCalled);
        }
    }
}