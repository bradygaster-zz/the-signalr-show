using Coding4Fun.Kinect.WinForm;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo5_ThereminController
{
    class Program
    {
        private const string HUB_URL = "http://localhost:8080/";
        HubConnection _connection;
        IHubProxy _hub;

        void Start()
        {
            _connection = new HubConnection(HUB_URL);

            _hub = _connection.CreateHubProxy("valueHub");

            _connection.Start().ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    Start();
                else
                    StartKinect();
            });
        }

        private Skeleton[] _skeletonData;
        public KinectSensor _kinect;

        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public Program()
        {
            _skeletonData = new Skeleton[0];
        }

        void StartKinect()
        {
            if (KinectSensor.KinectSensors.Any())
            {
                this._kinect = KinectSensor.KinectSensors.First();
                this._kinect.SkeletonStream.Enable();
                this._kinect.SkeletonFrameReady += Kinect_SkeletonFrameReady;
                this._kinect.AllFramesReady += Kinect_AllFramesReady;
                this._kinect.Start();
            }
        }

        void Stop()
        {
            Log("Stopping");
            this._kinect.Stop();
            Log("Stopped");
        }

        void Kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            _skeletonData.ToList().ForEach(s =>
            {
                if (s.TrackingState == SkeletonTrackingState.Tracked)
                {
                    var rightHand = s.Joints.First(x => x.JointType == JointType.HandRight);
                    var r = rightHand.ScaleTo(133, 100);

                    _hub.Invoke("changeValue", (int)r.Position.Y);

                    Log(string.Format("Right X: {0} Right Y: {1}",
                        r.Position.X,
                        r.Position.Y));
                }
            });

            Array.Clear(_skeletonData, 0, _skeletonData.Length);
        }

        void Kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                _skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

                if (skeletonFrame != null)
                {
                    skeletonFrame.CopySkeletonDataTo(_skeletonData);
                }
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            
            p.Start();
            Log("Press Enter to Quit");
            Console.ReadLine();
            Log("Press Enter to Exit");
            Console.ReadLine();
            p.Stop();
        }
    }
}
