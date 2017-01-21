using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [Serializable]
    public class Email
    {
        public int id { get; set; }
        public string sender { get; set; }
        public List<string> receiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime dateTime { get; set; }
        public bool isRead { get; set; }
        public string fileURI { get; set; }

        public Email()
        {
            receiver = new List<string>();
        }

        public Email(int id, string sender, List<string> receiver, string subject, string message, DateTime dateTime, bool isRead, string fileURI)
        {
            this.id = id;
            this.sender = sender;
            this.receiver = receiver;
            this.subject = subject;
            this.message = message;
            this.dateTime = dateTime;
            this.isRead = isRead;
            this.fileURI = fileURI;
        }

        public override string ToString()
        {
            return "Email:" +
                ",\n sender: " + sender + 
                ",\n receiver: " + receiver.ToString() + 
                ",\n subject: " + subject +
                ",\n dateTime: " + dateTime.ToString() +
                ",\n isRead: " + isRead +
                ",\n fileURI: " + fileURI +
                //",\n message: " + message +
                "";
        }
    }
}
