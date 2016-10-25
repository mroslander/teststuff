using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace ProtoPersistenceClient
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Workflow1();
            WorkflowInvoker.Invoke(workflow1);

            //TestWFServiceReference.Operation1Request r = new TestWFServiceReference.Operation1Request();
            
            Console.ReadLine();

            //TestWFServiceReference.IService  proxy = new ServiceReference1.ServiceClient();

            ////TestWFServiceReference.

            //Guid workflowId = (Guid)proxy.Operation1();

            //Console.WriteLine("client done");
            //Console.WriteLine("enter key");
            //Console.ReadLine();
        }
    }
}
