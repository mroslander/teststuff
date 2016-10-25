using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;
using System.Activities.DurableInstancing;
using System.ServiceModel;

namespace WorkflowConsoleProtoPersistence
{

    class Program
    {
        static void Main(string[] args)
        {
            //Activity workflow1 = new Workflow1();
            //WorkflowInvoker.Invoke(workflow1);

            //string baseAddress = "http://localhost:8080/TestWF";
            string baseAddress = "net.pipe://localhost/TestWF";

            using (WorkflowServiceHost host =
            new WorkflowServiceHost(new Workflow1(), new Uri(baseAddress)))
            {
                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                {
                    //HttpGetEnabled = true
                });

                host.AddServiceEndpoint("IService",
                    //new BasicHttpBinding(), 
                    new NetNamedPipeBinding(),
                    baseAddress);

                host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexNamedPipeBinding(),                
                    baseAddress + "/mex");

                SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore(
                   @"Data Source=.\SQLEXPRESS;Initial Catalog=" +
                   @"WorkflowInstanceStore;Integrated Security=True");
                host.DurableInstancingOptions.InstanceStore = instanceStore;

                host.Open();
                Console.WriteLine("Car rental service listening at: " +
                baseAddress);
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
        }
    }
}
