using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServiceHub
{
    [HubName("TestHub")]
    public class TestHub : Hub
    {       
        public void SendWorkflowInstanceRecord(string trackRecordData)
        {
            Clients.All.WorkflowInstanceRecordReceived(trackRecordData);
        }

        public void SendActivityStateRecord(string trackRecordData)
        {
            Clients.All.ActivityStateRecordReceived(trackRecordData);
        }

        public void SendCustomTrackingRecord(string trackRecordData)
        {
            Clients.All.CustomTrackingRecordReceived(trackRecordData);
        }
    }
}

