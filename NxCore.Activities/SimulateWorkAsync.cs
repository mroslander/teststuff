using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NxCore.Activities
{
    public class SimulateWorkAsync : AsyncCodeActivity<int>
    {
        public InArgument<int> Max { get; set; }

        public string WorkDescription { get; set; }

        static Random r = new Random();

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            // Create a delegate that references the method that implements
            // the asynchronous work. Assign the delegate to the UserState,
            // invoke the delegate, and return the resulting IAsyncResult.
            Func<int, int> DoWorkDelegate = new Func<int, int>(DoWork);
            context.UserState = DoWorkDelegate;

            //Console.WriteLine("Start instance " + context.ActivityInstanceId);

            return DoWorkDelegate.BeginInvoke(Max.Get(context), callback, state);
        }

        protected override int EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            // Get the delegate from the UserState and call EndInvoke
            Func<int, int> GetRandomDelegate = (Func<int, int>)context.UserState;

            int randomNumber = (int)GetRandomDelegate.EndInvoke(result);

            //Console.WriteLine("End instance " + context.ActivityInstanceId);
            TrackWorkResults(context);

            return randomNumber;
        }

        protected virtual void TrackWorkResults(AsyncCodeActivityContext context)
        {
            
        }

        protected virtual int DoWork(int max)
        {
            // This activity simulates taking a few moments. This code runs
            // asynchronously with respect to the workflow thread.
            Console.WriteLine("Start working on " + WorkDescription);

            System.Threading.Thread.Sleep(max);

            Console.WriteLine("Done working on " + WorkDescription);

            return r.Next(1, max + 1);
        }
    }
}
