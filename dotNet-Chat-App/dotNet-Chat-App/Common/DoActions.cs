namespace dotNet_Chat_App.Common
{
    public class DoActions
    {
        public enum Todo
        {
            PushLog,
            PushStatus,
            PushMessage, 
            PushOfflineMessage,
            PushOfflineGroupMessage
        }
        public enum MessageType
        {
            ServerSendAll = 10,
            ServerToSingleClient,
            ClientToServer,
            ClientToClient,
            OfflineSending,
            ChatToGroup
        }

        private DoActions() { }
    }
}
