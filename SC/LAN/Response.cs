using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public class Response
    {

        private byte[] responseByteArray;

        public byte[] ResponseByteArray
        {
            get { return responseByteArray; }
            set { responseByteArray = value; }
        }




        private eState _responseState;
        public eState ResponseState
        {
            get { return _responseState; }
          
        }

        public Response(byte[] responseParam)
        {

            this.ResponseByteArray = responseParam;
          
        }


        public Response(byte[] responseParam, int length)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                result.Add(responseParam[i]);
            }
            this.ResponseByteArray = result.ToArray();
           
        }

        public Response()
        {

        }

        public bool EvaluateResponse()
        {
            if (ResponseByteArray.Length == 0)
                return false;


            return true;
        }

    }
}
