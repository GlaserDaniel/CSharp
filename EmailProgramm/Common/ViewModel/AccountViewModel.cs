using Common.Model;
using Common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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
        //private ObservableCollection<EmailViewModel> _selectedEmailsToDelete;

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
            this.Showname = account.Showname;
            this.User = account.User;
            this.Email = account.Email;
            this.Password = account.Password;
            this.UseImap = account.UseImap;
            this.ImapPop3Server = account.ImapPop3Server;
            this.ImapPop3Port = account.ImapPop3Port;
            this.SmtpServer = account.SmtpServer;
            this.SmtpPort = account.SmtpPort;

            // List von Email zu ObservableCollection von EmailViewModel machen
            List<EmailViewModel> _emailsViewModel = new List<EmailViewModel>();
            List<Email> emails = account.Emails;
            foreach (var email in emails)
            {
                _emailsViewModel.Add(new EmailViewModel(email));
            }
            this.Emails = new ObservableCollection<EmailViewModel>(_emailsViewModel);
        }

        public AccountViewModel(string showname, string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort, ObservableCollection<EmailViewModel> emails)
        {
            this.Showname = showname;
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

        //public ObservableCollection<EmailViewModel> SelectedEmailsToDelete
        //{
        //    get { return _selectedEmailsToDelete; }
        //    set
        //    {
        //        if (_selectedEmailsToDelete == value) return;
        //        _selectedEmailsToDelete = value;
        //        OnPropertyChanged();
        //    }
        //}

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
            foreach (var email in _emails) // TODO kann hier abstürzen wegen InvalidOperationException (Die Auflistung wurde geändert. Der Enumerationsvorgang kann möglicherweise nicht ausgeführt werden.)
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
            if (Showname.Equals(""))
            {
                return Email.ToString();
            }
            return Showname.ToString();
        }

        public void DeleteSelectedCommandExecute(EmailViewModel emailToDelete)
        {
            // Email auf Server löschen
            new EmailService().deleteMessage(emailToDelete, AccountListViewModel.Instance.Accounts[AccountListViewModel.Instance.SelectedAccountIndex]);

            // Email aus Emails löschen
            Emails.Remove(emailToDelete);

            // TODO mehrere Emails löschen funktioniert noch nicht
            //foreach(var email in SelectedEmailsToDelete)
            //{
            //    Emails.Remove(email);
            //}
        }
    }
}
