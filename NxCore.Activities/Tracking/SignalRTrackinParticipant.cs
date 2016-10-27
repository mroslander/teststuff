using Microsoft.AspNet.SignalR.Client;
using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Activities.Tracking
{
    /// <summary>
    /// 
    /// </summary>
    /// 

    class ActivityStateRecordData
    {
        public ActivityInfo ActivityInfo { get; }
        //
        // Summary:
        //     Gets the current values of the tracked arguments associated with the activity
        //     when the record is generated.
        //
        // Returns:
        //     A System.Collections.Generic.IDictionary`2 containing the arguments.
        public IDictionary<string, object> Arguments { get; }
        //
        // Summary:
        //     Gets the current state of the activity when the record is generated.
        //
        // Returns:
        //     The activity state.
        public string State { get; }
        //
        // Summary:
        //     Gets the current values of the tracked variables associated with the activity
        //     when the record is generated.
        //
        // Returns:
        //     The current values.
        public IDictionary<string, object> Variables { get; }
    }

    class ActivityInfoData
    {
        //
        // Summary:
        //     Gets the ID for the activity.
        //
        // Returns:
        //     The activity ID.
        public string Id { get; }
        
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

    public class SignalRTrackinParticipant : TrackingParticipant
    {
        private IHubProxy _hub;

        

        // Tracking record received
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            if (_hub == null)
            {
            }

            WorkflowInstanceRecord workflowInstanceRecord = record as WorkflowInstanceRecord;
            if (workflowInstanceRecord != null)
            {
                _hub.Invoke("SendWorkflowInstanceRecord", Newtonsoft.Json.JsonConvert.SerializeObject(record)).Wait();
                //sw.WriteLine("------WorkflowInstanceRecord------");
                //sw.WriteLine("Workflow InstanceID: {0} Workflow instance state: {1}",
                //record.InstanceId, workflowInstanceRecord.State);
                //sw.WriteLine("\n");
            }

            ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
            if (activityStateRecord != null)
            {
                _hub.Invoke("SendActivityStateRecord", Newtonsoft.Json.JsonConvert.SerializeObject(record)).Wait();

                //IDictionary < string,object /> variables = activityStateRecord.Variables;
                //StringBuilder vars = new StringBuilder();

                //if (variables.Count > 0)
                //{
                //    vars.AppendLine("\n\tVariables:");
                //    foreach (KeyValuePair<string, object /> variable in variables)
                //    {
                //        vars.AppendLine(String.Format(
                //        "\t\tName: {0} Value: {1}", variable.Key, variable.Value));
                //    }
                //}

                //sw.WriteLine("------ActivityStateRecord------");
                //sw.WriteLine("Activity DisplayName: {0} :ActivityInstanceState: {1} {2}",
                //activityStateRecord.Activity.Name, activityStateRecord.State,
                //((variables.Count > 0) ? vars.ToString() : String.Empty));
                //sw.WriteLine("\n");
            }

            CustomTrackingRecord customTrackingRecord = record as CustomTrackingRecord;
            if ((customTrackingRecord != null)) // && (customTrackingRecord.Data.Count > 0))
            {
                _hub.Invoke("SendCustomTrackingRecord", Newtonsoft.Json.JsonConvert.SerializeObject(record)).Wait();
                //sw.WriteLine("------CustomTrackingRecord------");
                //sw.WriteLine("\n\tUser Data:");
                //foreach (string data in customTrackingRecord.Data.Keys)
                //{
                //    sw.WriteLine(" \t\t {0} : {1}", data, customTrackingRecord.Data[data]);
                //}
                //sw.WriteLine("\n");
            }
        }

        public SignalRTrackinParticipant(IHubProxy hub)
        {
            _hub = hub;
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



}
