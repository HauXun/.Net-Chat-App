using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IAction
{
    public interface IActions
    {
        void Close();
        void Listen();
        void Worker(object token = null);
        void WaitForHandle(object token = null);
        void SendMessage(MessageType type);
    }
}
