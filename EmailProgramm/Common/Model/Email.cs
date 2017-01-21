using Common.ViewModel;
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
        public int Id { get; set; }
        public string Sender { get; set; }
        public List<string> Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsHtml { get; set; }
        public string FileURI { get; set; }

        public Email()
        {
            Receiver = new List<string>();
        }

        public Email(EmailViewModel email)
        {
            this.Id = email.Id;
            this.Sender = email.Sender;
            this.Receiver = email.Receiver;
            this.Subject = email.Subject;
            this.Message = email.Message;
            this.DateTime = email.DateTime;
            this.IsRead = email.IsRead;
            this.IsHtml = email.IsHtml;
            this.FileURI = email.FileURI;
        }

        public Email(int id, string sender, List<string> receiver, string subject, string message, DateTime dateTime, bool isRead, string fileURI)
        {
            this.Id = id;
            this.Sender = sender;
            this.Receiver = receiver;
            this.Subject = subject;
            this.Message = message;
            this.DateTime = dateTime;
            this.IsRead = isRead;
            this.FileURI = fileURI;
        }

        public override string ToString()
        {
            return "Email:" +
                ",\n sender: " + Sender + 
                ",\n receiver: " + Receiver.ToString() + 
                ",\n subject: " + Subject +
                ",\n dateTime: " + DateTime.ToString() +
                ",\n isRead: " + IsRead +
                ",\n isHtml: " + IsHtml +
                ",\n fileURI: " + FileURI +
                //",\n message: " + message +
                "";
        }
    }
}
