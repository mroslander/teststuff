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
        public void DetermineLength(string message)
        {
            Console.WriteLine(message);

            string newMessage = string.Format(@"{0} has a length of: {1}", message, message.Length);
            Clients.All.ReceiveLength(newMessage);
        }

        public void TrackRecordReceived(string trackRecordData)
        {
            //Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityStateRecord
            Console.WriteLine("TrackRecordReceived {0}", trackRecordData);

            Clients.All.TrackRecordReceived(trackRecordData);
        }
    }
}

