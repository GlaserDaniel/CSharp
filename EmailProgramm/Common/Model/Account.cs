using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Account : INotifyPropertyChanged
    {
        public string user { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool useImap { get; set; }
        public string imapPop3Server { get; set; }
        public int imapPop3Port { get; set; }
        public string smtpServer { get; set; }
        public int smtpPort { get; set; }
        private ObservableCollection<Email> emails;

        public ObservableCollection<Email> Emails
        {
            get { return emails; }
            set
            {
                if (emails == value) return;
                emails = value;
                OnPropertyChanged("Emails");
            }
        }

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
            Emails = new ObservableCollection<Email>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return email.ToString();
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
