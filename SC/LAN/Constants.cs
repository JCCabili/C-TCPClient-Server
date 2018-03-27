using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public static class Constants
    {
        public static byte[] OUTOFSERVICE = { 0xC, 0x17, 0x5C, 0x1 };
        public static byte[] INSERVICE = { 0xC, 0x17, 0x5C, 0x2 };
        public static byte[] OFFLINE = { 0xC, 0x17, 0x5C, 0x3 };
        public static byte[] ONLINE = { 0xC, 0x17, 0x5C, 0x04 };
        public static byte[] MAINTENANCE = { 0xC, 0x17, 0x5C, 0x05 };

        public static byte[] TERMINALSTATUS = { 0xC, 0x17, 0x5C };
        //public static byte[] RECEIVE = { 0x7F, 0xa };
    }
}
