using NxCore.Activities;
using NxCore.Data;
using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RunWorkflow();
        }

        public void AppendText(string message)
        {
            textBoxTracRecords.AppendText(message);
        }

        public void RunWorkflow()
        {
            data = ProfileCuttingData.CreateTestData();
            
            AutonestProfileCuttingData wf = new AutonestProfileCuttingData();
            IDictionary<string, object> inputs = new Dictionary<string, object>();
            inputs[nameof(wf.Data)] = data;

            //WorkflowApplication wfapp = new WorkflowApplication(wf, inputs);
            WorkflowInvoker instance = new WorkflowInvoker(wf);            

            ////Mapping between the Object and Line No.
            //Dictionary<object, SourceLocation> wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService();

            ////Mapping between the Object and the Instance Id
            //Dictionary<string, Activity> activityIdToWfElementMap = BuildActivityIdToWfElementMap(wfElementToSourceLocationMap);

            #region Set up Custom Tracking
            const String all = "*";
            TextTrackingParticipant simTracker = new TextTrackingParticipant()
            {
                TrackingProfile = new TrackingProfile()
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
                }
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
                            Console.WriteLine((string.Format("  {0} = {1}, serialized: {2}", kv.Key, kv.Value, Newtonsoft.Json.JsonConvert.SerializeObject(kv.Value))));
                        }
                    }
                    else if (record is WorkflowInstanceRecord)
                    {
                        WorkflowInstanceRecord workflowInstanceRecord = record as WorkflowInstanceRecord;
                        msg = "== workflowInstanceRecord : " + workflowInstanceRecord.ToString();

                        // Workflowinstancerecord does not have activityInfo (its the workflow itself)


                    }
                    Console.WriteLine(msg);
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

            instance.Extensions.Add(simTracker);

            ThreadPool.QueueUserWorkItem(new WaitCallback((context) =>
            {
            //Invoking the Workflow Instance with Input Arguments
            instance.Invoke(inputs); // new Dictionary<string, object> { { "decisionVar", true } }, new TimeSpan(1, 0, 0));

                //This is to remove the final debug adornment
                this.Dispatcher.Invoke(DispatcherPriority.Render
                    , (Action)(() =>
                    {
                        
                    }));

            }));
        }
    } // MainWindow

    
}
