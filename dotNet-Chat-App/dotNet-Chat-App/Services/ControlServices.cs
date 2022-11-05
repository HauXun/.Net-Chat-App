using System.Windows.Forms;

namespace dotNet_Chat_App.Services
{
    public class ControlServices
    {
        public static object GetParent<T>(Control control)
        {
            while (control.Parent != null)
            {
                if (control.Parent is T)
                {
                    return (T)(object)control.Parent;
                }
                control = control.Parent;
            }
            return null;
        }
    }
}
