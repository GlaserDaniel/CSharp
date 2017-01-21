using System;
using System.Collections.Generic;

namespace Common.Model
{
    [Serializable]
    public class Account
    {
        public string Showname { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool UseImap { get; set; }
        public string ImapPop3Server { get; set; }
        public int ImapPop3Port { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public List<Email> Emails { get; set; }

        public Account()
        {
            Emails = new List<Email>();
        }

        public Account(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort, List<Email> emails)
        {
            this.User = user;
            this.Email = email;
            this.Password = password;
            this.UseImap = useImap;
            this.ImapPop3Server = imapPop3Server;
            this.ImapPop3Port = imapPop3Port;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Emails = emails;
        }

        public override string ToString()
        {
            return Showname.ToString();
        }

        public bool doEmailsContainsId(int id)
        {
            bool result = false;
            foreach (Email email in Emails)
            {
                if (email.id == id)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
