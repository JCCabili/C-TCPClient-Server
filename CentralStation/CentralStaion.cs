using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CentralStation.LAN.CentralStation;
namespace CentralStation
{
    public partial class CentralStaion : Form
    {
        public CentralStaion()
        {
            InitializeComponent();
            Initialize();
        }

        public Commands MyCommands;
        private StringBuilder Display;

        private void Initialize()
        {
            MyCommands = new Commands();
            Display = new StringBuilder();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ipaddress = txtIpAddress.Text;

            MyCommands.GetTerminalStatus(ipaddress);

            if (MyCommands.Response != null)
            {

                Display.AppendLine(string.Format("{0} - TX:{3} RX:{2} Val:{1} \n", ipaddress, MyCommands.Response.ResponseState.ToString(),MyCommands.RX,MyCommands.TX));

                LoadControls();


            }
        }

        private void LoadControls()
        {
            txtResponse.Text = Display.ToString();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {

            
            if (comboBox1.Text == "Shutdown")
            {
                MyCommands.TCShutdown(txtIpAddress.Text);
            }
            else if (comboBox1.Text == "Restart")
            {
                MyCommands.TCRestart(txtIpAddress.Text);
            }
            else if (comboBox1.Text == "MaintenanceMode")
            {
                MyCommands.TCMaintenanceMode(txtIpAddress.Text);
            }
            else if (comboBox1.Text == "OutOfServiceMode")
            {
                MyCommands.TCOutOfService(txtIpAddress.Text);
            }
            else if (comboBox1.Text == "InService")
            {
                MyCommands.TCInService(txtIpAddress.Text);
            }

            Display.AppendLine(string.Format("{0} - TX:{3} RX: {2} Command: {1} \n", txtIpAddress.Text, comboBox1.Text, MyCommands.RX,MyCommands.TX));
            LoadControls();
        }
    }
}
