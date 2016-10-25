using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;

namespace Spartan
{
    public class TextTrackingParticipant : System.Activities.Tracking.TrackingParticipant
    {
        public event EventHandler<TrackingEventArgs> TrackingRecordReceived;
        //public Dictionary<string, Activity> ActivityIdToWorkflowElementMap { get; set; }        

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            OnTrackingRecordReceived(record, timeout);
        }

        //On Tracing Record Received call the TrackingRecordReceived with the record received information from the TrackingParticipant. 
        //We also do not worry about Expressions' tracking data
        protected void OnTrackingRecordReceived(TrackingRecord record, TimeSpan timeout)
        {
            if (TrackingRecordReceived != null)
            {
                TrackingRecordReceived(this, new TrackingEventArgs(record, timeout, null));
            }
        }
    }

    //Custom Tracking EventArgs
    public class TrackingEventArgs : EventArgs
    {
        public TrackingRecord Record { get; set; }
        public TimeSpan Timeout { get; set; }
        public Activity Activity { get; set; }

        public TrackingEventArgs(TrackingRecord trackingRecord, TimeSpan timeout, Activity activity)
        {
            this.Record = trackingRecord;
            this.Timeout = timeout;
            this.Activity = activity;
        }
    }
}



