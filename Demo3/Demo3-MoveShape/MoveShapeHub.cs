using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Demo3_MoveShape
{
    [HubName("moveShape")]
    public class MoveShapeHub : Hub
    {
        public void MoveShape(double x, double y)
        {
            Clients.Others.shapeMoved(x, y);
        }
    }
}