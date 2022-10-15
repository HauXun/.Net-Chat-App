using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IAction
{
    public interface ITodo
    {
        void HandleAction(DoActions actions);

    }
}
