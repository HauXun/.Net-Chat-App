namespace dotNet_Chat_App.IAction
{
    public interface IActions
    {
        void Init();
        void Close();
        void Listen();
        void Worker(object token = null);
        void WaitForHandle(object token = null);
    }
}
