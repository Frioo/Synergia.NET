using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Message
    {
        public string ID { get; }
        public bool IsNote { get; } // warnings
        public bool IsDeleted { get; }
        public bool IsSent { get; } // originally 'isSended'
        public bool IsReceived { get; } // determines whether the message is sent or not
        public bool IsArchived { get; }
        public bool IsSpam { get; } // this is how Librus marks their own messages
        public string ReceiverID { get; }
        public string SenderID { get; }
        public int SendDate { get; }
        public int ReadDate { get; } // TODO: support multiple receivers/read dates
        public string Subject { get; } // topic
        public string Body { get; }
        public bool HasAttachments { get; }
        public PagesInfo PagesInfo;

        public Message(string id, bool isNote, bool isDeleted, bool isSent, bool isReceived, bool isArchived, 
            bool isSpam, string receiverID, string senderID, int sendDate, int readDate, 
            string subject, string body, bool hasAttachments, PagesInfo pagesInfo)
        {
            this.ID = id;
            this.IsNote = isNote;
            this.IsDeleted = isDeleted;
            this.IsSent = isSent;
            this.IsReceived = isReceived;
            this.IsArchived = isArchived;
            this.IsSpam = isSpam;
            this.ReceiverID = receiverID;
            this.SenderID = senderID;
            this.SendDate = sendDate;
            this.ReadDate = readDate;
            this.Subject = subject;
            this.Body = body;
            this.HasAttachments = hasAttachments;
            this.PagesInfo = pagesInfo;
        }
    }
}
