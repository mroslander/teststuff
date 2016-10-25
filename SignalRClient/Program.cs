using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IHubProxy _hub;
            string url = @"http://localhost:8080/";
            var connection = new HubConnection(url);
            _hub = connection.CreateHubProxy("TestHub");
            connection.Start().Wait();

            _hub.On("ReceiveLength", x => Console.WriteLine(x));

            string line = null;
            while ((line = System.Console.ReadLine()) != null)
            {
                _hub.Invoke("DetermineLength", line).Wait();
            }

            Console.Read();
        }
    }
}
