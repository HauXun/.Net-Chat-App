using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Utils
{
    public class DoActions
    {
        public enum Todo
        {
            PushStatus,
            PushOfflineMessage,
            PushOfflineGroupMessage
        }
        static public Todo TodoAction;
        static public MessageType MsgType;

    }
}
