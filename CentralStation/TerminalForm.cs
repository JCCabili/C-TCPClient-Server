using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CentralStation.LAN.Terminal;
using CentralStation.Custom;
using CentralStation.LAN;
using System.Diagnostics;

namespace CentralStation
{
    public partial class TerminalForm : Form
    {
        public TerminalForm()
        {
            InitializeComponent();
            Initialize();
        }

        public TerminalClient Client = null;
        public void Initialize()
        {
            Client = new TerminalClient();
            CentralStaion CS = new CentralStaion();
            CS.Show();
            LoadControls();


            Client.CentralCommand += new CommandsEventHandler(Client_CentralCommand);
        }

        private void Client_CentralCommand(object sender, TerminalEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(RunCommand), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void RunCommand(TerminalEventArgs args)
        {

            label3.Text = args.Response.CentralCommand.ToString();

            eCentralCommand ReceiveCommand = args.Response.CentralCommand;

            if (ReceiveCommand == eCentralCommand.Shutdown)
            {
               // MessageBox.Show("System Will shutdown...");
                Process.Start("shutdown", "/s /t 0");
            }
            else if (ReceiveCommand == eCentralCommand.Restart)
            {
               // MessageBox.Show("System Will restart...");
                Process.Start("shutdown", "/r /t 0");
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Global.IPaddress = txtIpadd.Text;

            Client.StartListeningEvents();
            label1.Text = "StartListening";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client.RequestStop();
            label1.Text = "StopListening";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Client.Response.ResponseState = LAN.eState.InService;
            LoadControls();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Client.Response.ResponseState = LAN.eState.Maintenance;
            LoadControls();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Client.Response.ResponseState = LAN.eState.OutofService;
            LoadControls();
        }

        private void LoadControls()
        {
            label2.Text = Client.Response.ResponseState.ToString();
        }

        private void TerminalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.RequestStop();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Client.Response.IsHasCurrentTransaction = true;
            }
            else
                Client.Response.IsHasCurrentTransaction = false;
        }
    }
}
