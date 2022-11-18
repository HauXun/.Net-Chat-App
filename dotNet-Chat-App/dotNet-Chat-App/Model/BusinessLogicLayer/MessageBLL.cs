﻿using dotNet_Chat_App.Common;
using dotNet_Chat_App.Model.DataAccessLayer;
using dotNet_Chat_App.Utils;
using System.Collections.Generic;
using System.Data;

namespace dotNet_Chat_App.Model.BusinessLogicLayer
{
    public class MessageBLL
    {
        private volatile static MessageBLL instance;

        private MessageBLL()
        {

        }
        public static MessageBLL Instance
        {
            get
            {
                if (instance == null)
                    instance = new MessageBLL();
                return instance;
            }
            set => instance = value;
        }

        public bool SaveMessage(TransactionPacket packets, int sent, int iDReceiver, int iDSent = 1)
        {
            return MessageDAL.Instance.SaveMessage(packets, sent, iDReceiver, iDSent);
        }

        public List<Message> GetAllMessage()
        {
            DataTable data = MessageDAL.Instance.GetAllMessage();

            if (data != null && data.Rows.Count > 0)
            {
                List<Message> results = new List<Message>();
                foreach (DataRow row in data.Rows)
                {
                    results.Add(new Message(row));
                }
                return results;
            }

            return null;
        }

        public List<Message> GetMessageBothSide(int clientSent, int clientRec)
        {
            DataTable data = MessageDAL.Instance.GetMessageBothSide(clientSent, clientRec);

            if (data != null && data.Rows.Count > 0)
            {
                List<Message> results = new List<Message>();
                foreach (DataRow row in data.Rows)
                {
                    results.Add(new Message(row));
                }
                return results;
            }

            return null;
        }

        public List<Message> GetAllMessageUnsent()
        {
            DataTable data = MessageDAL.Instance.GetAllMessageUnsent();

            if (data != null && data.Rows.Count > 0)
            {
                List<Message> results = new List<Message>();
                foreach (DataRow row in data.Rows)
                {
                    results.Add(new Message(row));
                }
                return results;
            }

            return null;
        }

        public bool FecthMessageStatus(int messageId, int status)
        {
            return MessageDAL.Instance.FecthMessageStatus(messageId, status);
        }
    }
}
