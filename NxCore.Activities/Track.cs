using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Activities
{
    public class Track : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> Name { get; set; }
        public InArgument<System.Diagnostics.TraceLevel> TraceLevel { get; set; }
        public InArgument<Dictionary<string, object>> Data { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            CustomTrackingRecord customTrackRecord = new CustomTrackingRecord(context.GetValue(Name), context.GetValue(TraceLevel));
            if (TraceLevel == null)
            {

            }
            //foreach (KeyValuePair<string, object> kv in context.GetValue(Data))
            //{
            //    customTrackRecord.Data[kv.Key] = kv.Value;
            //}
            context.Track(customTrackRecord);
        }


    }
    
}
