﻿using System;
using System.Collections.Generic;

namespace Common.Model
{
    /// <summary>
    /// Stellt einen E-Email-Account dar
    /// </summary>
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

        /// <summary>
        /// Standard Konstruktor für einen Account
        /// </summary>
        public Account()
        {
            Emails = new List<Email>();
        }

        /// <summary>
        /// Konstruktor für einen Account dem alles gesetzt werden kann.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="useImap"></param>
        /// <param name="imapPop3Server"></param>
        /// <param name="imapPop3Port"></param>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <param name="emails"></param>
        /// <param name="signature"></param>
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
    }
}
