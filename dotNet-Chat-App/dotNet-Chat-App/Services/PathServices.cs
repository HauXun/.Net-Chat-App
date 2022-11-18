using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Services
{
    public class PathServices
    {
        private static string appDirectory;
        public static string AppDirectory 
        { 
            get
            {
                if (string.IsNullOrEmpty(appDirectory))
                {
                    appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    int idxOfBin = appDirectory.IndexOf("\\bin");
                    appDirectory = appDirectory.Substring(0, idxOfBin > 0 ? idxOfBin : 0);
                }
                return appDirectory;
            }
        }
    }
}
