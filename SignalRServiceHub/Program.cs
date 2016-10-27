using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServiceHub
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"http://+:8080/";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine(string.Format("Server running at {0}", url));
                Console.ReadLine();
            }
        }
    }
}
