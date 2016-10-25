using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;

namespace NxCore.Activities
{

    public sealed class ImportProfilePart : SimulateWorkAsync
    {
        protected override void TrackWorkResults(AsyncCodeActivityContext context)
        {
            CustomTrackingRecord resultRecord = new CustomTrackingRecord("result", System.Diagnostics.TraceLevel.Info);
            resultRecord.Data["SomeImportDataKey"] = "SomeImportDataKey";
            context.Track(resultRecord);
        }
    }
}
