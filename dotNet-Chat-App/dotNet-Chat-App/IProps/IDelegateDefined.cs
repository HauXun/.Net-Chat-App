using dotNet_Chat_App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.IProps
{
	public interface IDelegateDefined
	{
		ClientListChanged ClientListChanged { get; set; }
		ClearClientListContainer ClearClientListContainer { get; set; }
	}
}
