using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Email
    {
        public string sender { get; set; }
        public string receiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime dateTime { get; set; }
        public bool isRead { get; set; }
        public string fileURI { get; set; }

        public Email()
        {

        }

        public override string ToString()
        {
            return "sender: " + sender + ", receiver: " + receiver + ", subject: " + subject + ", message: " + message + ", fileURI: " + fileURI;
        }
    }
}
