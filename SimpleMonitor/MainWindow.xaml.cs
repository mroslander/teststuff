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
using System.Windows.Threading;

namespace SimpleMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITrackingEventMonitor
    {
        private string baseAddress = @"http://matti.nestix.fi:8080/";
        private IHubProxy Hub;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hub = ConnectToServiceHub(baseAddress);

            // Set events 
            Hub.On<string>("ActivityStateRecordReceived", ActivityStateRecordReceived);
            Hub.On<string>("CustomTrackingRecordReceived", CustomTrackingRecordReceived);
            Hub.On<string>("WorkflowInstanceRecordReceived", WorkflowInstanceRecordReceived);
        }

        private List<TrackingRecord> allRecords = new List<TrackingRecord>();

        public void ActivityStateRecordReceived(string data)
        {
            ActivityStateRecord activityStateRecord = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityStateRecord>(data);
            AddActivityStateRecord(activityStateRecord);
        }

        private void AddTrackingRecord(TrackingRecord trackingRecord)
        {            
            AllRecords.Add(trackingRecord);
        }

        Dictionary<string, ViewModel.AutonestActivityPresenter> ActivityViewsById = 
            new Dictionary<string, ViewModel.AutonestActivityPresenter>();

        public List<TrackingRecord> AllRecords
        {
            get
            {
                if (allRecords == null)
                {
                    allRecords = new List<TrackingRecord>();
                }
                return allRecords;
            }

            set
            {
                allRecords = value;
            }
        }

        ViewModel.AutonestActivityPresenter GetActivityView(CustomTrackingRecord record)
        {
            if (record.Activity.TypeName.ToLower().Contains("nestlib"))
            {

            }
            else
            {

            }
            ViewModel.AutonestActivityPresenter activityView = null;
            if (ActivityViewsById.TryGetValue(record.Activity.InstanceId, out activityView) == false)
            {
                activityView = CreateView(record);
                ActivityViewsById[record.Activity.InstanceId] = activityView;
            }

            return activityView;
        }

        private void AddActivityStateRecord(ActivityStateRecord activityStateRecord)
        {           
            types.Add(activityStateRecord.Activity.TypeName);
        }
        List<string> types = new List<string>();
        public void CustomTrackingRecordReceived(string data)
        {
            CustomTrackingRecord customTrackingRecord = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomTrackingRecord>(data);
            AddCustomTrackingRecord(customTrackingRecord);
        }

        public ViewModel.AutonestActivityPresenter CreateView(CustomTrackingRecord trackingRecord)
        {
            ViewModel.AutonestActivityPresenter view = new ViewModel.AutonestActivityPresenter();

            Views.AutonestActivityView v = new Views.AutonestActivityView();
            v.DataContext = view;

            canvas.Children.Add(v);

            x += 80;

            Canvas.SetLeft(v, x);

            return view;
        }
        int x = 0;
        private void AddCustomTrackingRecord(CustomTrackingRecord customTrackingRecord)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                ViewModel.AutonestActivityPresenter activityView = GetActivityView(customTrackingRecord);

                if (activityView != null)
                {

                }
                else
                {
                    Console.WriteLine("No view for activity {0}. Type is {1}, Id {2}",
                        customTrackingRecord.Activity.Name, customTrackingRecord.Activity.TypeName, customTrackingRecord.Activity.Id);
                }
                activityView.AddTrackingRecord(customTrackingRecord);
                AddTrackingRecord(customTrackingRecord);
                Console.WriteLine(customTrackingRecord.Activity.TypeName);
            }));
            
        }

        public void WorkflowInstanceRecordReceived(string data)
        {
            WorkflowInstanceRecord workflowInstanceRecordData = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowInstanceRecord>(data);
            AddWorkflowInstanceRecord(workflowInstanceRecordData);
            //Console.WriteLine(data);
        }

        private void AddWorkflowInstanceRecord(WorkflowInstanceRecord workflowInstanceRecordData)
        {
            AddTrackingRecord(workflowInstanceRecordData);
        }

        public IHubProxy ConnectToServiceHub(string baseAddress) //, TimeSpan timeout = TimeSpan.Zero)
        {
            IHubProxy hub;
            var connection = new HubConnection(baseAddress);
            hub = connection.CreateHubProxy("TestHub");
            connection.Start().Wait();

            return hub;
        }
    }

    public interface ITrackingEventMonitor
    {
        void ActivityStateRecordReceived(string data);
        void CustomTrackingRecordReceived(string data);
        void WorkflowInstanceRecordReceived(string data);
    }


    public class TrackingRecord
    {
        //
        // Summary:
        //     When implemented in a derived class, represents a collection of name/value pairs
        //     that are added to this tracking record.
        //
        // Returns:
        //     The dictionary of elements to be added to this tracking record.
        public IDictionary<string, string> Annotations { get; set; }
        //
        // Summary:
        //     When implemented in a derived class, represents the time the tracking record
        //     occurred.
        //
        // Returns:
        //     The event time.
        public DateTime EventTime { get; set; }
        //
        // Summary:
        //     When implemented in a derived class, represents the ID of the generating workflow
        //     instance.
        //
        // Returns:
        //     The element ID.
        public Guid InstanceId { get; set; }
        //
        // Summary:
        //     Gets the System.Diagnostics.TraceLevel of the event.
        //
        // Returns:
        //     The trace level.
        public TraceLevel Level { get; set; }
        //
        // Summary:
        //     A sequence that defines the order in which tracking records are generated.
        //
        // Returns:
        //     The sequence order.
        public long RecordNumber { get; set; }       
    }

    //
    // Summary:
    //     Specifies what messages to output for the System.Diagnostics.Debug, System.Diagnostics.Trace
    //     and System.Diagnostics.TraceSwitch classes.
    public enum TraceLevel
    {
        //
        // Summary:
        //     Output no tracing and debugging messages.
        Off = 0,
        //
        // Summary:
        //     Output error-handling messages.
        Error = 1,
        //
        // Summary:
        //     Output warnings and error-handling messages.
        Warning = 2,
        //
        // Summary:
        //     Output informational messages, warnings, and error-handling messages.
        Info = 3,
        //
        // Summary:
        //     Output all debugging and tracing messages.
        Verbose = 4
    }

    //
    // Summary:
    //     Represents a tracking record that is created when an activity changes state.
    public sealed class ActivityStateRecord : TrackingRecord
    {       
        //
        // Summary:
        //     Gets an System.Activities.Tracking.ActivityInfo that contains information on
        //     the activity when the record is generated.
        //
        // Returns:
        //     The activity information.
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

    
    //
    // Summary:
    //     Contains information on a tracked System.Activities.Activity.
    public sealed class ActivityInfo
    {
       
        //
        // Summary:
        //     Gets the ID for the activity.
        //
        // Returns:
        //     The activity ID.
        public string Id { get; set; } 
        //
        // Summary:
        //     Gets the run-time ID of the activity instance.
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

    //
    // Summary:
    //     Contains the data sent to a tracking participant by the run-time tracking infrastructure
    //     when a custom tracking record is raised.
    public class CustomTrackingRecord : TrackingRecord
    {       
        //
        // Summary:
        //     Returns the System.Activities.Tracking.ActivityInfo of the activity associated
        //     with this record.
        //
        // Returns:
        //     The activity information.
        public ActivityInfo Activity { get; set; } 
        //
        // Summary:
        //     Gets the user-defined data associated with this tracking record.
        //
        // Returns:
        //     The user-defined data.
        public IDictionary<string, object> Data { get; set; } 
        //
        // Summary:
        //     Gets the name that distinguishes this tracking record.
        //
        // Returns:
        //     The name that distinguishes this tracking record.
        public string Name { get; set; } 
    }


    //
    // Summary:
    //     Contains the data sent to a tracking service by the run-time tracking infrastructure
    //     when a workflow instance changes state.
    public class WorkflowInstanceRecord : TrackingRecord
    {        
        //
        // Summary:
        //     Returns the display name of the root activity of the workflow that generated
        //     this record.
        //
        // Returns:
        //     The activity definition ID.
        public string ActivityDefinitionId { get; set; } 
        //
        // Summary:
        //     The current state of the workflow when the record is generated.
        //
        // Returns:
        //     The workflow state.
        public string State { get; set; } 
        //
        // Summary:
        //     Gets or sets the information about the workflow identity.
        //
        // Returns:
        //     The information about the workflow identity.
        public WorkflowIdentity WorkflowDefinitionIdentity { get; protected set; }

    }

    //
    // Summary:
    //     Maps a persisted workflow instance to its corresponding workflow definition.
    public class WorkflowIdentity
    {
        //
        // Summary:
        //     Gets or sets the name of the workflow identity.
        //
        // Returns:
        //     The name of the workflow identity.
        public string Name { get; set; }
        //
        // Summary:
        //     Gets or sets the package of the workflow identity.
        //
        // Returns:
        //     The package of the workflow identity.
        public string Package { get; set; }
        //
        // Summary:
        //     Gets or sets the version of the workflow identity.
        //
        // Returns:
        //     The version of the workflow identity.
        public Version Version { get; set; }      
    }    
}
