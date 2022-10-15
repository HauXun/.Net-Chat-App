using dotNet_Chat_App.IAction;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Core
{
    public class CCore : IActions, ITodo
    {
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Listen()
        {
            throw new NotImplementedException();
        }

        public void HandleAction(DoActions actions)
        {
            throw new NotImplementedException();
        }

        public void WaitForHandle(object token = null)
        {
            throw new NotImplementedException();
        }

        public void Worker(object token = null)
        {
            throw new NotImplementedException();
        }
    }
}
