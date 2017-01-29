using Common.ViewModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// Stellt eine E-Mail dar
    /// </summary>
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
        public List<string> Attachments { get; set; }
        public int AccountIndex { get; set; }

        /// <summary>
        /// Standard Konstruktor für eine Email
        /// </summary>
        public Email()
        {
            Receiver = new List<string>();
            Attachments = new List<string>();
        }

        /// <summary>
        /// Konstruktor der aus ein EmailViewModel annimmt
        /// </summary>
        /// <param name="email"></param>
        public Email(EmailViewModel email)
        {
            Id = email.Id;
            AccountIndex = email.AccountIndex;
            Sender = email.Sender;
            Receiver = email.Receivers;
            Subject = email.Subject;
            Message = email.Message;
            DateTime = email.DateTime;
            IsRead = email.IsRead;
            IsHtml = email.IsHtml;
            Attachments = email.Attachments;
        }

        /// <summary>
        /// Konstruktor mit dem alles gesetzt werden kann
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountIndex"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="dateTime"></param>
        /// <param name="isRead"></param>
        /// <param name="attachments"></param>
        public Email(int id, int accountIndex, string sender, List<string> receiver, string subject, string message, DateTime dateTime, bool isRead, List<string> attachments)
        {
            Id = id;
            AccountIndex = accountIndex;
            Sender = sender;
            Receiver = receiver;
            Subject = subject;
            Message = message;
            DateTime = dateTime;
            IsRead = isRead;
            Attachments = attachments;
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
                ",\n attachments: " + Attachments +
                //",\n message: " + message +
                "";
        }
    }
}
