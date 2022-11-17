using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Services
{
    public class JsonServices
    {
        public static void JSONEncode(List<Message> messages)
        {
            var serilizedObject = Newtonsoft.Json.JsonConvert.SerializeObject(messages, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(PathServices.AppDirectory + @"\Cache\msg.json"))
            {
                sw.Write(serilizedObject);
            }
        }

        public static List<Message> JSONDecode()
        {
            string resultJson = string.Empty;
            using (StreamReader sr = new StreamReader(PathServices.AppDirectory + @"\Cache\msg.json"))
            {
                resultJson = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<Message>>(resultJson);
        }
    }
}
