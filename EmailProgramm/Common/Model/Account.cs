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
        public string Signature { get; set; }

        public Account()
        {
            Emails = new List<Email>();
        }

        public Account(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort, List<Email> emails, string signature)
        {
            User = user;
            Email = email;
            Password = password;
            UseImap = useImap;
            ImapPop3Server = imapPop3Server;
            ImapPop3Port = imapPop3Port;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            Emails = emails;
            Signature = signature;
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
                if (email.Id == id)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
