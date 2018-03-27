using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public class StationComputerClient : StationComputerBase
    {
        TcpListener server = null;

        //private Response Response;
        private byte[] bytes;
        private string data;

        //public eState eStateResponse { get; set; }
        public Response Response { get; set; }


        public StationComputerClient()
        {
            bytes = new Byte[256];
            data = string.Empty;
            this.Response = new Response();
        }

        public bool Open()
        {
            Int32 port = 8008;

            
            if (PingAddress(Global.IPaddress))
            {
                IPAddress localAddress = IPAddress.Parse(Global.IPaddress);
                server = new TcpListener(localAddress, port);
                server.Start();
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
                    this.Response = new Response(bytes, i);


                    Debug.WriteLine("Recieved:" + data);
                    stream.Write(Constants.RECEIVE,0, Constants.RECEIVE.Length);
                    


                    byte[] state = new byte[2];
                     Array.Copy(bytes, state, 2);


                    if (state.SequenceEqual(Constants.INSERVICE))
                        Response.ResponseState = eState.InService;
                    else if (state.SequenceEqual(Constants.MAINTENANCE))
                        Response.ResponseState = eState.Maintenance;
                    else if (state.SequenceEqual(Constants.OFFLINE))
                        Response.ResponseState = eState.Offline;
                    else if (state.SequenceEqual(Constants.ONLINE))
                        Response.ResponseState = eState.Online;
                    else if (state.SequenceEqual(Constants.OUTOFSERVICE))
                        Response.ResponseState = eState.OutofService;
                    else
                        Response.ResponseState = eState.Invalid;




                }
               
                return true;
            }
            else
            {
                return false;
            }
        }

        //public static byte[] OUTOFSERVICE = { 0x7F, 0x1 };
        //public static byte[] INSERVICE = { 0x7F, 0x2 };
        //public static byte[] OFFLINE = { 0x7F, 0x3 };

        //public static byte[] ONLINE = { 0x7F, 0x04 };
        //public static byte[] MAINTENANCE = { 0x7F, 0x05 };


        //public static byte[] RECEIVE = { 0x7F, 0xa };



        public bool SendCommand(string ip,byte[] status)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;

            try
            {
                tcp = new TcpClient();

                tcp.SendTimeout = 500;
                tcp.ReceiveTimeout = 500;
                tcp.Connect(IPAddress.Parse(ip), 8008);
                ns = tcp.GetStream();
                ns.Write(status,0, status.Length);

                Byte[] response = new Byte[256];
                int byteCount = ns.Read(response, 0,256);


                string responseData = System.Text.Encoding.ASCII.GetString(response,0,byteCount);

                Byte[] result = TrimByteArray(response,byteCount);
                Debug.WriteLine("Terminal SendControl:" + ip);
                Debug.WriteLine("Send:" + BitConverter.ToString(status));
                Debug.WriteLine("Receive:" + BitConverter.ToString(result));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                    ns.Close();
                tcp.Close();
            }

        }

        #region Utility
        public static bool PingAddress(string host)
        {
            if (host == string.Empty) return false;

            Ping x = new Ping();
            PingReply pingReply = x.Send(IPAddress.Parse(host));
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] TrimByteArray(byte[] received, int receiveLength)
        {
            byte[] result = new byte[receiveLength];

            Array.Copy(received, result, receiveLength);

            return result;
        }


        #endregion
    }
}
