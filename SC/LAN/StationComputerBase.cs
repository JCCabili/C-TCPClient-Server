﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SC.LAN
{
    delegate void RunControllerProcess(StationComputerEventArgs args);

    public class StationComputerEventArgs : EventArgs
    {
        public Response Response { get; set; }
        public StationComputerEventArgs(Response response)
        {
            Response = response;
        }

    }

    public delegate void TerminalEventHandler(object sender, StationComputerEventArgs e);

    public class StationComputerBase
    {
        private Thread workerThread;
        public event TerminalEventHandler TerminalStateChange = null;
     

        protected void OnTerminalStateChange(StationComputerEventArgs e)
        {
            if (this.TerminalStateChange != null)
                this.TerminalStateChange(this, e);
        }



        public eState CurrentState { get; set; }


        StationComputerClient client;
        public void Do_Work()
        {


            client = new StationComputerClient();

            CurrentState = eState.Offline;

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
                            OnTerminalStateChange(new StationComputerEventArgs(client.Response));
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
