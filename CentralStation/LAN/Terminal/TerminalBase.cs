using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralStation.LAN.Terminal
{
    delegate void RunControllerProcess(TerminalEventArgs args);

    public class TerminalEventArgs : EventArgs
    {
        public ResponseTerminal Response { get; set; }
        public TerminalEventArgs(ResponseTerminal response)
        {
            Response = response;
        }

    }

    public delegate void TerminalEventHandler(object sender, TerminalEventArgs e);

    public delegate void CommandsEventHandler(object sender, TerminalEventArgs e);

    public class TerminalBase
    {
        private Thread workerThread;
        public event TerminalEventHandler TerminalStateChange = null;
        public event CommandsEventHandler CentralCommand = null;

        protected void OnTerminalStateChange(TerminalEventArgs e)
        {
            if (this.TerminalStateChange != null)
                this.TerminalStateChange(this, e);
        }


        protected void OnRecieveCommand(TerminalEventArgs e)
        {
            if (this.CentralCommand != null)
                this.CentralCommand(this, e);
        }


        public eState CurrentState { get; set; }
        public eCentralCommand eCommands { get; set; }


        TerminalClient client;
        public void Do_Work()
        {


            client = new TerminalClient();

            CurrentState = eState.Offline;
            eCommands = eCentralCommand.Idle;


            if (client.Open())
            {
                while (!_shouldStop)
                {

                    if (!_shouldStop && client.AcceptClient())
                    {
                        System.Diagnostics.Debug.WriteLine("Do_Work");


                        if (CurrentState != client.Response.ResponseState)
                        {
                            CurrentState = client.Response.ResponseState;
                            OnTerminalStateChange(new TerminalEventArgs(client.Response));
                        }

                        if (eCommands != client.Response.CentralCommand)
                        {
                            eCommands = client.Response.CentralCommand;
                            OnRecieveCommand(new TerminalEventArgs(client.Response));
                        }

                    }
                }
            }



        }


        public void StartListeningEvents()
        {

            workerThread = new Thread(this.Do_Work);
            workerThread.Start();
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " main thread: Starting worker thread..");

            while (!workerThread.IsAlive) ;
            Thread.Sleep(1);

        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Keyword volatile is used as a hint to the compiler that this data
        // member is accessed by multiple threads.
        private volatile bool _shouldStop;


    }


}
