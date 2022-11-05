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
            PushOfflineGroupMessage,
            PushSyncAddGroup,
            PushGroupList
        }
        public enum MessageType
        {
            ServerSendAll = 100,
            ServerToSingleClient,
            ClientToServer,
            ClientToClient,
            OfflineSending,
            ChatToGroup
        }

        private DoActions() { }
    }
}
