using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using System.ComponentModel;

namespace NxCore.Activities
{
    [DesignerCategory("Profile Cutting")]
    public sealed class AutonestProfileJobWithNestlib : SimulateWorkAsync
    {
        //public int TimePerNestingResult { get; set; }
        public InArgument<int> ResultNo { get; set; }

        protected override void TrackWorkResults(AsyncCodeActivityContext context)
        {
            // TODO Track overall statistics of this nesting process

            CustomTrackingRecord resultRecord = new CustomTrackingRecord("result", System.Diagnostics.TraceLevel.Info);
            resultRecord.Data["SheetResult"] = "Sheetresult" + ResultNo.Get(context);
            context.Track(resultRecord);
        }

        //protected override int DoWork(int max)
        //{            
        //    Console.WriteLine("Start working on " + WorkDescription);

        //    for (int i = 0; i < max; i++)
        //    {
                

        //        System.Threading.Thread.Sleep(TimePerNestingResult);
        //    }            

        //    Console.WriteLine("Done working on " + WorkDescription);

        //    return 1;
        //}
    }
}
