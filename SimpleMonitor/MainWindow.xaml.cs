using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITrackingEventMonitor
    {
        private string baseAddress = @"http://localhost:8080/";
        private IHubProxy Hub;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hub = ConnectToServiceHub(baseAddress);

            // Set events 
            Hub.On<string>("TrackRecordReceived", OnTrackingEvent);
            //{
            //    Console.WriteLine("JEEJEEJEEJE" + r );
            //});
        }

        public IHubProxy ConnectToServiceHub(string baseAddress) //, TimeSpan timeout = TimeSpan.Zero)
        {
            IHubProxy hub;
            var connection = new HubConnection(baseAddress);
            hub = connection.CreateHubProxy("TestHub");
            connection.Start().Wait();

            return hub;
        }

        public void OnTrackingEvent(string data)
        {
            object o = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            ActivityStateRecord activityStateRecord = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityStateRecord>(data);
            Console.WriteLine("JEEJEEJEEJE" + data);
        }

        //IHubProxy _hub;
        //string url = @"http://localhost:8080/";
        //var connection = new HubConnection(url);
        //_hub = connection.CreateHubProxy("TestHub");
        //    connection.Start().Wait();

        //_hub.On("ReceiveLength", x => Console.WriteLine(x));

        //    string line = null;
        //    while ((line = System.Console.ReadLine()) != null)
        //    {
        //        _hub.Invoke("DetermineLength", line).Wait();
    }

    public interface ITrackingEventMonitor
    {
        void OnTrackingEvent(string data);
    }

    class ActivityStateRecord
    {
        public ActivityInfo Activity { get; set; }
        //
        // Summary:
        //     Gets the current values of the tracked arguments associated with the activity
        //     when the record is generated.
        //
        // Returns:
        //     A System.Collections.Generic.IDictionary`2 containing the arguments.
        public IDictionary<string, object> Arguments { get; set; }
        //
        // Summary:
        //     Gets the current state of the activity when the record is generated.
        //
        // Returns:
        //     The activity state.
        public string State { get; set; }
        //
        // Summary:
        //     Gets the current values of the tracked variables associated with the activity
        //     when the record is generated.
        //
        // Returns:
        //     The current values.
        public IDictionary<string, object> Variables { get; set; }
    }

    class ActivityInfo
    {
        //
        // Summary:
        //     Gets the ID for the activity.
        //
        // Returns:
        //     The activity ID.
        public string Id { get; set; }

        // Gets the run-time ID of the activity instance.
        //
        // Returns:
        //     The instance ID of the activity.

        public string InstanceId { get; set; }
        //
        // Summary:
        //     Gets the name associated with the activity
        //
        // Returns:
        //     The activity name.
        public string Name { get; set; }

        //
        // Summary:
        //     Gets the type name of the activity.
        //
        // Returns:
        //     The type name of the activity.
        public string TypeName { get; set; }
    }
}
