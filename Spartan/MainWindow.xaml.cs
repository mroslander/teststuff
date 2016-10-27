using Microsoft.AspNet.SignalR.Client;
using NxCore.Activities;
using NxCore.Data;
using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.DurableInstancing;
using System.Text;
using System.Threading;
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

namespace Spartan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProfileCuttingData data;

        private IHubProxy _hub;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Connect to service hub
            IHubProxy hub = null;
            var connection = new HubConnection("http://localhost:8080/");
            hub = connection.CreateHubProxy("TestHub");
            connection.Start().Wait();
            _hub = hub;
            // Set events
            _hub.On<string>(nameof(DecisionReceived), DecisionReceived);

            RunWorkflow();
        }

        public void AppendText(string message)
        {
            textBoxTracRecords.AppendText(message);
        }

        //public string ServiceHubBaseAddress { get; set; }

        public void RunWorkflow()
        {
            // Data for workflow
            data = ProfileCuttingData.CreateTestData();

            // Workflow activity
            //AutonestProfileCuttingData wf = new AutonestProfileCuttingData();
            var wf = new NxCore.Activities.SampleWorkflows.FromImportToNestings();
            IDictionary<string, object> inputs = new Dictionary<string, object>();
            inputs[nameof(wf.ImportTask)] = ImportTask.CreateTestTask();
            //inputs[nameof(wf.Data)] = data;


            // Workflow app to run activity
            WorkflowApplication app = new WorkflowApplication(wf, inputs);

            #region Setup persisting

            //InstanceStore store = new SqlWorkflowInstanceStore(@"Data Source=.\SQLEXPRESS;Initial Catalog=" +
            //    @"WorkflowInstanceStore;Integrated Security=True");

            //InstanceHandle handle = store.CreateInstanceHandle();
            //InstanceView view = store.Execute(handle,
            //  new CreateWorkflowOwnerCommand(), TimeSpan.FromSeconds(30));
            //handle.Free();
            //store.DefaultInstanceOwner = view.InstanceOwner;
            //app.InstanceStore = store;

            #endregion

            //WorkflowApplication wfapp = new WorkflowApplication(wf, inputs);


            ////Mapping between the Object and Line No.
            //Dictionary<object, SourceLocation> wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService();

            ////Mapping between the Object and the Instance Id
            //Dictionary<string, Activity> activityIdToWfElementMap = BuildActivityIdToWfElementMap(wfElementToSourceLocationMap);

            #region Set up Custom Tracking
            const String all = "*";

            TrackingProfile TrackingProfile = new TrackingProfile()
            {
                Name = "CustomTrackingProfile",
                Queries =
                        {
                            new CustomTrackingQuery()
                            {
                                Name = all,
                                ActivityName = all
                            },
                            new WorkflowInstanceQuery()
                            {
                                // Limit workflow instance tracking records for started and completed workflow states
                                States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                            },
                            new ActivityStateQuery()
                            {
                                // Subscribe for track records from all activities for all states
                                ActivityName = all,
                                States = { all },

                                // Extract workflow variables and arguments as a part of the activity tracking record
                                // VariableName = "*" allows for extraction of all variables in the scope
                                // of the activity
                                Variables =
                                {
                                    { all }
                                }
                            }
                            
                        }
            };

            TextTrackingParticipant simTracker = new TextTrackingParticipant()
            {
                TrackingProfile = TrackingProfile
            };

            #endregion

            //simTracker.ActivityIdToWorkflowElementMap = activityIdToWfElementMap;

            //As the tracking events are received
            simTracker.TrackingRecordReceived += (trackingParticpant, trackingEventArgs) =>
            {
                ActivityInfo a = null;
                TrackingRecord record = trackingEventArgs.Record;

                this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                {
                    string msg = "";
                    if (record is ActivityStateRecord)
                    {
                        ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
                        msg = "== activityStateRecord  : " + activityStateRecord.ToString();

                        a = activityStateRecord.Activity;
                        //messages.Add(Newtonsoft.Json.JsonConvert.SerializeObject(a));
                    }
                    else if (record is CustomTrackingRecord)
                    {
                        CustomTrackingRecord customTrackRecord = record as CustomTrackingRecord;
                        msg = "== customTrackRecord : " + customTrackRecord.ToString();

                        a = customTrackRecord.Activity;
                        //messages.Add(Newtonsoft.Json.JsonConvert.SerializeObject(a));
                        foreach (KeyValuePair<string, object> kv in customTrackRecord.Data)
                        {
                            msg += string.Format("\n    {0} = {1}   (serialized value: {2})", kv.Key, kv.Value,
                                Newtonsoft.Json.JsonConvert.SerializeObject(kv.Value));
                        }
                    }
                    else if (record is WorkflowInstanceRecord)
                    {
                        WorkflowInstanceRecord workflowInstanceRecord = record as WorkflowInstanceRecord;
                        msg = "== workflowInstanceRecord : " + workflowInstanceRecord.ToString();

                        // Workflowinstancerecord does not have activityInfo (its the workflow itself)


                    }
                    //Console.WriteLine(msg);
                    AppendText(msg + "\n");


                    ////Textbox Updates
                    //tx.AppendText(trackingEventArgs.Activity.DisplayName + " " + ((ActivityStateRecord)trackingEventArgs.Record).State + "\n");
                    //tx.AppendText("******************\n");
                    //textLineToSourceLocationMap.Add(i, wfElementToSourceLocationMap[trackingEventArgs.Activity]);
                    //i = i + 2;
                    //Console.WriteLine("DISPATHCER ========================================================");
                    //foreach (string message in messages)
                    //{
                    //    //textBlockTrackingData.append.Text += message + "\n";
                    //    
                    //}
                    //Add a sleep so that the debug adornments are visible to the user
                    System.Threading.Thread.Sleep(10);
                }));

                //if (trackingEventArgs.Activity != null)
                //{
                //    System.Diagnostics.Debug.WriteLine(
                //        String.Format("<+=+=+=+> Activity Tracking Record Received for ActivityId: {0}, record: {1} ",
                //        trackingEventArgs.Activity.Id,
                //        trackingEventArgs.Record
                //        )
                //    );

                //    //ShowDebug(wfElementToSourceLocationMap[trackingEventArgs.Activity]);

                //    this.Dispatcher.Invoke(DispatcherPriority.SystemIdle, (Action)(() =>
                //    {
                //        ////Textbox Updates
                //        //tx.AppendText(trackingEventArgs.Activity.DisplayName + " " + ((ActivityStateRecord)trackingEventArgs.Record).State + "\n");
                //        //tx.AppendText("******************\n");
                //        //textLineToSourceLocationMap.Add(i, wfElementToSourceLocationMap[trackingEventArgs.Activity]);
                //        //i = i + 2;

                //        //Add a sleep so that the debug adornments are visible to the user
                //        System.Threading.Thread.Sleep(10);
                //    }));

                //}
            };
            #region SignalR tracking

            NxCore.Activities.Tracking.SignalRTrackinParticipant signalRTracker = new NxCore.Activities.Tracking.SignalRTrackinParticipant(_hub)
            {
                TrackingProfile = TrackingProfile
            };

            #endregion

            //string test = Newtonsoft.Json.JsonConvert.SerializeObject(wf);

            //// Workflow Description
            ////WorkflowDesigner
            //PropertyInfo[] ps = wf.GetType().GetProperties();


            if (_hub != null)
            {
                app.Extensions.Add(signalRTracker);
            }

            app.Extensions.Add(simTracker);

            

            ThreadPool.QueueUserWorkItem(new WaitCallback((context) =>
            {
                AutoResetEvent sync = new AutoResetEvent(false);

                app.Completed += (e) =>
                {
                    Console.WriteLine("Completed " + e);
                    sync.Set();
                };

                app.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
                {
                    Console.WriteLine(e.Reason);
                    sync.Set();
                };

                app.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    Console.WriteLine(e.UnhandledException.ToString());
                    return UnhandledExceptionAction.Terminate;
                };

                app.Run();

                sync.WaitOne();
            }));

            //ThreadPool.QueueUserWorkItem(new WaitCallback((context) =>
            //{
            ////Invoking the Workflow Instance with Input Arguments
            //instance.Invoke(inputs); // new Dictionary<string, object> { { "decisionVar", true } }, new TimeSpan(1, 0, 0));

            //    //This is to remove the final debug adornment
            //    this.Dispatcher.Invoke(DispatcherPriority.Render
            //        , (Action)(() =>
            //        {

            //        }));

        //}));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RunWorkflow();
        }

        private void buttonRequestDecision_Click(object sender, RoutedEventArgs e)
        {
            string data = "paatosdataa1";

            // Send decision request to hub
            AppendText("Send decision request to hub " + data);
            _hub.Invoke("RequestDecision", data);
        }

        public void DecisionReceived(string data)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                AppendText("Decision Received " + data);
            }));
        }

    } // MainWindow

    
}
