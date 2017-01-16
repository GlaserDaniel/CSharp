using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private string _showname;
        private string _user;
        private string _email;
        private string _password;
        private bool _useImap;
        private string _imapPop3Server;
        private int _imapPop3Port;
        private string _smtpServer;
        private int _smtpPort;
        private ObservableCollection<EmailViewModel> _emails;

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator] TODO
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountViewModel()
        {

        }

        public AccountViewModel(Account account)
        {
            Showname = account.Showname;
            User = account.User;
            Email = account.Email;
            Password = account.Password;
            UseImap = account.UseImap;
            ImapPop3Server = account.ImapPop3Server;
            ImapPop3Port = account.ImapPop3Port;
            SmtpServer = account.SmtpServer;
            SmtpPort = account.SmtpPort;
            Emails = new ObservableCollection<EmailViewModel>();
        }

        public AccountViewModel(string showname, string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort)
        {
            Showname = showname;
            User = user;
            Email = email;
            Password = password;
            UseImap = useImap;
            ImapPop3Server = imapPop3Server;
            ImapPop3Port = imapPop3Port;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            Emails = new ObservableCollection<EmailViewModel>();
        }

        public ObservableCollection<EmailViewModel> Emails
        {
            get { return _emails; }
            set
            {
                if (_emails == value) return;
                _emails = value;
                OnPropertyChanged();
            }
        }

        public string Showname
        {
            get
            {
                return _showname;
            }
            set
            {
                if (_showname == value) return;
                _showname = value;
                OnPropertyChanged();
            }
        }

        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user == value) return;
                _user = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ImapPop3Server
        {
            get { return _imapPop3Server; }
            set
            {
                if (_imapPop3Server == value) return;
                _imapPop3Server = value;
                OnPropertyChanged();
            }
        }
        
        public string SmtpServer
        {
            get { return _smtpServer; }
            set
            {
                if (_smtpServer == value) return;
                _smtpServer = value;
                OnPropertyChanged();
            }
        }

        public bool UseImap
        {
            get { return _useImap; }
            set
            {
                if (_useImap == value) return;
                _useImap = value;
                OnPropertyChanged();
            }
        }

        public int ImapPop3Port
        {
            get { return _imapPop3Port; }
            set
            {
                if (_imapPop3Port == value) return;
                _imapPop3Port = value;
                OnPropertyChanged();
            }
        }

        public int SmtpPort
        {
            get { return _smtpPort; }
            set
            {
                if (_smtpPort == value) return;
                _smtpPort = value;
                OnPropertyChanged();
            }
        }

        public bool doEmailsContainsId(int id)
        {
            bool result = false;
            foreach (var email in Emails)
            {
                if (email.Id == id)
                {
                    result = true;
                }
            }
            return result;
        }

        public override string ToString()
        {
            return Email.ToString();
        }
    }
}
