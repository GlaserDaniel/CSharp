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
        public String sender { get; set; }
        public String receiver { get; set; }
        public String subject { get; set; }
        public String message { get; set; }
        public String fileURI { get; set; }

        public Email()
        {

        }

        public override string ToString()
        {
            return "sender: " + sender + ", receiver: " + receiver + ", subject: " + subject + ", message: " + message + ", fileURI: " + fileURI;
        }
    }
}
