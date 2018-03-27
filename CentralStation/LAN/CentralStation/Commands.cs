using CentralStation.Custom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CentralStation.LAN.CentralStation
{
    public partial class Commands
    {

        public ResponseTerminal Response { get; set; }
        //public string RX { get; set; }
        public string RX { get; set; }
        public string TX { get; set; }
        public Commands()
        {
            this.Response = new ResponseTerminal();
        }


        public bool GetTerminalStatus(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            TX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                    Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.TERMINALSTATUS, 0, Constants.TERMINALSTATUS.Length);
                    TX = BitConverter.ToString(Constants.TERMINALSTATUS);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);


                   


                    byte[] state = new byte[4];
                    Array.Copy(response, state, 4);

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

                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);



                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));


                }
                else
                {
                    Response.ResponseState = eState.Offline;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }  
            }

        }

        public bool TCShutdown(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                    //Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.CSHUTDOWN, 0, Constants.CSHUTDOWN.Length);
                    TX = BitConverter.ToString(Constants.CSHUTDOWN);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);

                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);


                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }
            }

        }

        public bool TCRestart(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                    //Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.CRESTART, 0, Constants.CRESTART.Length);
                    TX = BitConverter.ToString(Constants.CRESTART);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);


                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);

                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }
            }

        }

        public bool TCMaintenanceMode(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                   // Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.CMAINTENANCE, 0, Constants.CMAINTENANCE.Length);
                    TX = BitConverter.ToString(Constants.CMAINTENANCE);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);

                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);


                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }
            }

        }

        public bool TCOutOfService(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                    //Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.COUTOFSERVICE, 0, Constants.COUTOFSERVICE.Length);
                    TX = BitConverter.ToString(Constants.COUTOFSERVICE);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);

                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);


                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }
            }

        }

        public bool TCInService(string ip)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;
            RX = string.Empty;
            try
            {
                if (Utilities.PingAddress(ip))
                {
                    // If ping success state will be now online.
                    //Response.ResponseState = eState.Online;


                    tcp = new TcpClient();
                    tcp.SendTimeout = 500;
                    tcp.ReceiveTimeout = 500;
                    tcp.Connect(IPAddress.Parse(ip), 8008);
                    ns = tcp.GetStream();
                    ns.Write(Constants.CINSERVICE, 0, Constants.CINSERVICE.Length);
                    TX = BitConverter.ToString(Constants.CINSERVICE);

                    Byte[] response = new Byte[256];
                    int byteCount = ns.Read(response, 0, 256);
                    string responseData = System.Text.Encoding.ASCII.GetString(response, 0, byteCount);

                    byte[] byteresponse = new byte[5];
                    Array.Copy(response, byteresponse, 5);
                    RX = BitConverter.ToString(byteresponse);

                    Byte[] result = Utilities.TrimByteArray(response, byteCount);
                    Debug.WriteLine("Terminal SendControl:" + ip);
                    Debug.WriteLine("Send:" + BitConverter.ToString(Constants.TERMINALSTATUS));
                    Debug.WriteLine("Receive:" + BitConverter.ToString(result));
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                {
                    ns.Close();
                    tcp.Close();
                }
            }

        }
    }
}

