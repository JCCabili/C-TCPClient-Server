using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CentralStation.Custom
{
    public static class Utilities
    {
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
    }
}
