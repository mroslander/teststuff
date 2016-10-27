using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitor.ViewModel
{
    public class AutonestActivityPresenter : ObservableObject
    {
        private readonly ObservableCollection<CustomTrackingRecord> _trackingRecords = new ObservableCollection<CustomTrackingRecord>();

        private CustomTrackingRecord _lastRecord = null;
        //public 

        public string Name
        {
            get
            {
                if (_lastRecord == null)
                {
                    return "-";
                }
                return _lastRecord.Activity.Name;
            }
            set
            {
            //    _someText = value;
            //    RaisePropertyChangedEvent("SomeText");
            }
        }

        public string State
        {
            get
            {
                // TODO state from data

                return "State" + _trackingRecords.Count;
            }
            set
            {

            }
        }

        public void AddTrackingRecord(CustomTrackingRecord record)
        {                     
            _trackingRecords.Add(record);
            _lastRecord = record;

            RaisePropertyChangedEvent("Name");
            RaisePropertyChangedEvent("State");
            RaisePropertyChangedEvent("TrackingRecords");
        }

        public IEnumerable<CustomTrackingRecord> TrackingRecords
        {
            get { return _trackingRecords; }
        }

        //public ICommand ConvertTextCommand
        //{
        //    get { return new DelegateCommand(ConvertText); }
        //}

        //private void ConvertText()
        //{
        //    if (string.IsNullOrWhiteSpace(SomeText)) return;
        //    AddToHistory(_textConverter.ConvertText(SomeText));
        //    SomeText = string.Empty;
        //}

        

        


    }
}
