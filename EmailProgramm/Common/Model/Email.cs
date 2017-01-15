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
        public int hashCode { get; set; }
        public string sender { get; set; }
        public IList<string> receiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime dateTime { get; set; }
        public bool isRead { get; set; }
        public string fileURI { get; set; }

        public Email()
        {
            receiver = new List<string>();
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
