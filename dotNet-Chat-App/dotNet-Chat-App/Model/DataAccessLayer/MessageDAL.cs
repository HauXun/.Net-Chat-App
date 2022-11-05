using dotNet_Chat_App.Common;
using dotNet_Chat_App.Services;
using dotNet_Chat_App.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Model.DataAccessLayer
{
    public class MessageDAL
    {
        private volatile static MessageDAL instance;

        private MessageDAL()
        {

        }
        public static MessageDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new MessageDAL();
                return instance;
            }
            set => instance = value;
        }

        public bool SaveMessage(TransactionPacket packets, int sent, int iDReceiver, int iDSent = 1)
        {
            byte[] packetSent = FragmentationServices.Serialize(packets);
            string query = "EXEC USP_SaveMessage @MessageDetail , @ClientSent , @ClientReceiver , @Sent , @MessageType";

            return DataProvider.Instance.ExcuteNonQuery(query, new object[]
            {
                packetSent,
                iDSent,
                iDReceiver,
                sent,
                packets.Todo
            }) > 0;
        }

        public DataTable GetAllMessage()
        {
            string query = "SELECT * FROM Messages";
            return DataProvider.Instance.ExcuteQuery(query);
        }
    }
}
