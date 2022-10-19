using dotNet_Chat_App.Common;
using System.Net.Sockets;

namespace dotNet_Chat_App.IAction
{
    public interface ITodo
    {
        void HandleAction(TransactionPacket packet, Socket socket);
        void SendPacket(TransactionPacket packet);
        void SendPacket(TransactionPacket packet, Socket socket);
    }
}
