using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [Serializable]
    public class Account : INotifyPropertyChanged
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

        public Account()
        {

        }

        public Account(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort)
        {
            this.User = user;
            this.Email = email;
            this.Password = password;
            this.UseImap = useImap;
            this.ImapPop3Server = imapPop3Server;
            this.ImapPop3Port = imapPop3Port;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            Emails = new ObservableCollection<Email>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
