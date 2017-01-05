using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Account
    {
        public string user { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool useImap { get; set; }
        public string imapPop3Server { get; set; }
        public int imapPop3Port { get; set; }
        public string smtpServer { get; set; }
        public int smtpPort { get; set; }
        public ArrayList emails { get; set; }

        public Account(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort)
        {
            this.user = user;
            this.email = email;
            this.password = password;
            this.useImap = useImap;
            this.imapPop3Server = imapPop3Server;
            this.imapPop3Port = imapPop3Port;
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.emails = new ArrayList();
        }

        public override string ToString()
        {
            return email.ToString();
        }
    }
}
