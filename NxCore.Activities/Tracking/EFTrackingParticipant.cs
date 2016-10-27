using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Activities.Tracking
{
    public class EFTrackingParticipant : TrackingParticipant
    {
        public EFTrackingParticipant()
        {

        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            // TODO Create TrackingRecordData-object and add to DatbaseContext & SaveChanges()
        }

        
    }
}
