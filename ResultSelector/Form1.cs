using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResultSelector
{
    // eventnames?
    //public const string 

    public partial class Form1 : Form, IDecisionMaker
    {
        private IHubProxy _hub = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IHubProxy hub = null;
            var connection = new HubConnection(@"http://localhost:8080/");
            hub = connection.CreateHubProxy("TestHub");
            connection.Start().Wait();

            hub.On<string>(nameof(DecisionRequestReceived), DecisionRequestReceived);

            _hub = hub;
        }

        
        public void DecisionRequestReceived(string data)
        {

            // Send decision to hub
            _hub.Invoke("SendDecision", data);
        }

        // This client sends 

    }

    public interface IDecisionMaker
    {
        void DecisionRequestReceived(string data);
    }
    
}
