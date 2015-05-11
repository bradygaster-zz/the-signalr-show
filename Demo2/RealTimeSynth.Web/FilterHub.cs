using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LittleBits.CloudBitApiClient;
using System.Configuration;

namespace RealTimeSynth.Web
{
    public class FilterHub : Hub
    {
        private string ACCESS_TOKEN = ConfigurationManager.AppSettings["ACCESS_TOKEN"];
        private string DEVICE_ID = ConfigurationManager.AppSettings["DEVICE_ID"];

        private static List<UserState> _userStateCollection = new List<UserState>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            if (!_userStateCollection.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                _userStateCollection.Add(new UserState
                    {
                        ConnectionId = Context.ConnectionId,
                        IsActive = false
                    });
            }

            UpdateSynth();

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            _userStateCollection.RemoveAll(x => x.ConnectionId == Context.ConnectionId);

            UpdateSynth();

            return base.OnDisconnected(stopCalled);
        }

        public void Activate()
        {
            _userStateCollection.Find(x => x.ConnectionId == Context.ConnectionId).IsActive = true;

            UpdateSynth();
        }

        public void Deactivate()
        {
            _userStateCollection.Find(x => x.ConnectionId == Context.ConnectionId).IsActive = false;

            UpdateSynth();
        }

        public void UpdateSynth()
        {
            var userCount = (float)_userStateCollection.Count;

            if (userCount == 0)
                return;

            var activeCount = (float)_userStateCollection.Where(x => x.IsActive == true).Count();
            var inActiveCount = (float)_userStateCollection.Where(x => x.IsActive == false).Count();
            var activePct = (activeCount / userCount) * 100F;
            var inActivePct = (inActiveCount / userCount) * 100F;

            Clients.All.valueChanged(new
            {
                userCount = userCount,
                activePct = (int)activePct,
                inActivePct = (int)inActivePct,
            });

            Client
                .Authenticate(ACCESS_TOKEN)
                .SetOutput(new DeviceOutputRequest
                {
                    Percent = (decimal)activePct,
                    DeviceId = DEVICE_ID,
                    DurationInMilliseconds = -1
                });
        }
    }

    public class UserState
    {
        public string ConnectionId { get; set; }
        public bool IsActive { get; set; }
    }
}