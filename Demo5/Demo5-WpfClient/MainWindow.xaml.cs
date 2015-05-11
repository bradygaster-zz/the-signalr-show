using Microsoft.AspNet.SignalR.Client;
using System.Windows;

namespace Demo5_WpfClient
{
    public partial class MainWindow : Window
    {
        IHubProxy _hub;

        public MainWindow()
        {
            InitializeComponent();

            _slider.ValueChanged += (s, e) => {
                _hub.Invoke("changeValue", (int)e.NewValue);
            };

            this.Loaded += async (s, e) => {
                var connection = new HubConnection("http://localhost:8080/");
                _hub = connection.CreateHubProxy("valueHub");
                await connection.Start();
            };
        }
    }
}
