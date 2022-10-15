using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Utils
{
    public class MessageType
    {
        private volatile static MessageType instance;

        private MessageType()
        {

        }
        public enum MsgType
        {
            ServerSendAll,
            ServerToSingleClient,
            OfflineSending,
            ClientToClient,
        }

        public static MessageType Instance
        {
            get
            {
                if (instance == null)
                    instance = new MessageType();
                return instance;
            }
            set => instance = value;
        }

    }
}
