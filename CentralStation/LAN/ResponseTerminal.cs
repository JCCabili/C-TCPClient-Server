using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralStation.LAN
{
    public class ResponseTerminal
    {
        private byte[] receiveByteArray;

        public byte[] RecieveByteArray
        {
            get { return receiveByteArray; }
            set { receiveByteArray = value; }
        }


        private static byte[] reponseByteArray;

        public byte[] ReponseByteArray
        {
            get { return reponseByteArray; }
            set { reponseByteArray = value; }
        }


        public static bool _ishasCurrentTransaction;
        public bool IsHasCurrentTransaction
        {
            get { return _ishasCurrentTransaction; }
            set { _ishasCurrentTransaction = value; }
        }

        private static eState _responseState;
        public eState ResponseState
        {
            get { return _responseState; }
            set { _responseState = value; }
        }


        private static eCentralCommand _centralCommand;
        public eCentralCommand CentralCommand
        {
            get { return _centralCommand; }
            set { _centralCommand = value; }
        }






        public ResponseTerminal(byte[] recieve, int length)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                result.Add(recieve[i]);
            }
            this.RecieveByteArray = result.ToArray();

        }

        public ResponseTerminal()
        {

        }

        public bool EvaluateResponse()
        {
            if (RecieveByteArray.Length == 0)

                return false;
            else
            {

                byte[] state = new byte[4];
                Array.Copy(RecieveByteArray, state, 4);


                //byte[] response = new byte[4];


                if (state.SequenceEqual(Constants.TERMINALSTATUS))
                {
                    if (this.ResponseState == eState.InService)
                        ReponseByteArray = Constants.INSERVICE;
                    else if (this.ResponseState == eState.Maintenance)
                        ReponseByteArray = Constants.MAINTENANCE;
                    else if (this.ResponseState == eState.Offline)
                        ReponseByteArray = Constants.OFFLINE;
                    else if (this.ResponseState == eState.Online)
                        ReponseByteArray = Constants.ONLINE;
                    else if (this.ResponseState == eState.OutofService)
                        ReponseByteArray = Constants.OUTOFSERVICE;
                }

                else if (state.SequenceEqual(Constants.TERMINALCOMMAND))
                {
                    if (!this.IsHasCurrentTransaction)
                    {
                        byte[] command = new byte[5];
                        Array.Copy(RecieveByteArray, command, 5);
                        if (command.SequenceEqual(Constants.CSHUTDOWN))
                        {
                            this.CentralCommand = eCentralCommand.Shutdown;
                        }
                        else if (command.SequenceEqual(Constants.CRESTART))
                        {
                            this.CentralCommand = eCentralCommand.Restart;
                        }
                        else if (command.SequenceEqual(Constants.COUTOFSERVICE))
                        {
                            this.CentralCommand = eCentralCommand.OutofServiceMode;
                            this.ResponseState = eState.OutofService;
                        }
                        else if (command.SequenceEqual(Constants.CMAINTENANCE))
                        {
                            this.CentralCommand = eCentralCommand.MaintenanceMode;
                            this.ResponseState = eState.Maintenance;
                        }
                        else if (command.SequenceEqual(Constants.CINSERVICE))
                        {
                            this.CentralCommand = eCentralCommand.InService;
                            this.ResponseState = eState.InService;
                        }
                        ReponseByteArray = Constants.CRECEIVE;
                    }
                    else
                    {
                        ReponseByteArray = Constants.CIGNORE;
                    }

                }
                return true;
            }
         
        }
    }
}
