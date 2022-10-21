using dotNet_Chat_App.Common;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IAction
{
    public interface ITodo
    {
        void HandleAction(TransactionPacket packet, Socket socket);
        Task SendPacket(TransactionPacket packet);
        Task SendPacket(TransactionPacket packet, Socket socket);
    }
}
