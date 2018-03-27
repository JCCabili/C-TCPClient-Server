using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CentralStation.Custom;

namespace CentralStation.LAN.Terminal
{
    public class TerminalClient : TerminalBase
    {
        TcpListener server = null;

        //private Response Response;
        private byte[] bytes;
        private string data;

        //public eState eStateResponse { get; set; }
        public ResponseTerminal Response { get; set; }


        public TerminalClient()
        {
            bytes = new Byte[256];
            data = string.Empty;
            this.Response = new ResponseTerminal();
        }

        public bool Open()
        {
            Int32 port = 8008;


            if (Utilities.PingAddress(Global.IPaddress))
            {
                IPAddress localAddress = IPAddress.Parse(Global.IPaddress);
                server = new TcpListener(localAddress, port);
                server.Start();
                //this.Response.ResponseState = eState.Online;
                return true;
            }
            else
                return false;

        }



        public bool AcceptClient()
        {
            if (server.Pending())
            {
                TcpClient client = server.AcceptTcpClient();
                data = null;

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = BitConverter.ToString(bytes, 0, i);
                    this.Response = new ResponseTerminal(bytes, 5);
                    this.Response.EvaluateResponse();

                    Debug.WriteLine("Recieved:" + data);



                    byte[] response = new byte[4];
                    response = this.Response.ReponseByteArray;
                    stream.Write(response, 0, response.Length);
                    Debug.WriteLine("Response:" + BitConverter.ToString(response, 0, response.Length));

                    #region OldResponse



                    //byte[] state = new byte[4];
                    //Array.Copy(bytes, state, 4);


                    //byte[] response = new byte[4];

                    //// Status handler
                    //if (state.SequenceEqual(Constants.TERMINALSTATUS))
                    //{
                    //    if (Response.ResponseState == eState.InService)
                    //        response = Constants.INSERVICE;
                    //    else if (Response.ResponseState == eState.Maintenance)
                    //        response = Constants.MAINTENANCE;
                    //    else if (Response.ResponseState == eState.Offline)
                    //        response = Constants.OFFLINE;
                    //    else if (Response.ResponseState == eState.Online)
                    //        response = Constants.ONLINE;
                    //    else if (Response.ResponseState == eState.OutofService)
                    //        response = Constants.OUTOFSERVICE;
                    //}
                    //// Command Handler
                    //else if (state.SequenceEqual(Constants.TERMINALCOMMAND))
                    //{
                    //    if (!Response.IsHasCurrentTransaction)
                    //    {
                    //        byte[] command = new byte[5];
                    //        Array.Copy(bytes, command, 5);
                    //        if (command.SequenceEqual(Constants.CSHUTDOWN))
                    //        {
                    //            Response.CentralCommand = eCentralCommand.Shutdown;
                    //        }
                    //        else if (command.SequenceEqual(Constants.CRESTART))
                    //        {
                    //            Response.CentralCommand = eCentralCommand.Restart;
                    //        }
                    //        else if (command.SequenceEqual(Constants.COUTOFSERVICE))
                    //        {
                    //            Response.CentralCommand = eCentralCommand.OutofServiceMode;
                    //            Response.ResponseState = eState.OutofService;
                    //        }
                    //        else if (command.SequenceEqual(Constants.CMAINTENANCE))
                    //        {
                    //            Response.CentralCommand = eCentralCommand.MaintenanceMode;
                    //            Response.ResponseState = eState.Maintenance;
                    //        }
                    //        else if (command.SequenceEqual(Constants.CINSERVICE))
                    //        {
                    //            Response.CentralCommand = eCentralCommand.InService;
                    //            Response.ResponseState = eState.InService;
                    //        }
                    //        response = Constants.CRECEIVE;
                    //    }
                    //    else
                    //    {
                    //        response = Constants.CIGNORE;
                    //    }

                    //}

                    //stream.Write(response, 0, response.Length);
                    //Debug.WriteLine("Response:" + BitConverter.ToString(response, 0, response.Length));
                    #endregion
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
