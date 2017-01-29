using Common.Exceptions;
using Common.Model;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections;
using System.Net.Mail;

namespace Common.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged, INotifyDataErrorInfo, IEditableObject
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

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
        private string _signature;
        //private ObservableCollection<EmailViewModel> _selectedEmailsToDelete;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //[NotifyPropertyChangedInvocator] TODO
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountViewModel()
        {
            UseImap = true;
            Emails = new ObservableCollection<EmailViewModel>();
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

            // List von Email zu ObservableCollection von EmailViewModel machen
            List<EmailViewModel> _emailsViewModel = new List<EmailViewModel>();
            List<Email> emails = account.Emails;
            foreach (var email in emails)
            {
                _emailsViewModel.Add(new EmailViewModel(email));
            }
            Emails = new ObservableCollection<EmailViewModel>(_emailsViewModel);

            Signature = account.Signature;
        }

        public AccountViewModel(string showname, string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort, ObservableCollection<EmailViewModel> emails, string signature)
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
            Emails = emails;
            Signature = signature;
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

        public string Signature
        {
            get { return _signature; }
            set
            {
                if (_signature == value) return;
                _signature = value;
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
                if (IsShownameValid(value))
                {
                    _showname = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool IsShownameValid(String showname)
        {
            var error = "Bitte geben Sie einen Anzeigenamen ein.";
            if (String.IsNullOrEmpty(showname))
            {
                AddError(nameof(Showname), error, false);
                return false;
            }
            RemoveError(nameof(Showname), error);
            return true;
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
                if (IsUserValid(value))
                {
                    _user = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool IsUserValid(String user)
        {
            var error = "Bitte geben Sie einen Benutzername ein.";
            if (String.IsNullOrEmpty(user))
            {
                AddError(nameof(User), error, false);
                return false;
            }
            RemoveError(nameof(User), error);
            return true;
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
                if (IsEmailValid(value))
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool IsEmailValid(String email)
        {
            var error = "Bitte geben Sie eine Email-Adresse ein.";
            if (String.IsNullOrEmpty(email))
            {
                AddError(nameof(Email), error, false);
                return false;
            }
            try
            {
                new MailAddress(email);
            }
            catch (FormatException)
            {
                AddError(nameof(Email), error, false);
                return false;
            }
            RemoveError(nameof(Email), error);
            return true;
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                if (IsPasswordValid(value))
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool IsPasswordValid(String password)
        {
            var error = "Bitte geben Sie ein Passwort ein.";
            if (String.IsNullOrEmpty(password))
            {
                AddError(nameof(Password), error, false);
                return false;
            }
            RemoveError(nameof(Password), error);
            return true;
        }

        public string ImapPop3Server
        {
            get { return _imapPop3Server; }
            set
            {
                if (_imapPop3Server == value) return;
                if (IsImapPop3ServerValid(value))
                {
                    _imapPop3Server = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool IsImapPop3ServerValid(String imapPop3Server)
        {
            var error = "Bitte geben Sie einen Imap/Pop3-Server ein.";
            if (String.IsNullOrEmpty(imapPop3Server))
            {
                AddError(nameof(ImapPop3Server), error, false);
                return false;
            }
            RemoveError(nameof(ImapPop3Server), error);
            return true;
        }

        public string SmtpServer
        {
            get { return _smtpServer; }
            set
            {
                if (_smtpServer == value) return;
                if (IsSmtpServerValid(value))
                {
                    _smtpServer = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool IsSmtpServerValid(String smtpServer)
        {
            var error = "Bitte geben Sie einen SMTP-Server ein.";
            if (String.IsNullOrEmpty(smtpServer))
            {
                AddError(nameof(SmtpServer), error, false);
                return false;
            }
            RemoveError(nameof(SmtpServer), error);
            return true;
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

        /// <summary>
        /// Prüfen ob eine Email mit der übergebenden ID bereits vorhanden ist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool doEmailsContainsId(int id)
        {
            bool result = false;

            List<int> ids = new List<int>();

            try
            {
                // Eine neue Liste erstellen da es hier sonst abstürtz
                ids = new List<int>(_emails.Select(email => email.Id).ToList());
            }
            catch (InvalidOperationException)
            {
                // falls der Error trotzdem auftreten sollte, lieber abfangen
                Console.WriteLine("InvalidOperationException in doEmailsContainsId");
                return false;
            }

            foreach (var _id in ids)
            {
                if (_id == id)
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
            new EmailService().deleteMessageAsync(emailToDelete, AccountListViewModel.Instance.Accounts[AccountListViewModel.Instance.SelectedAccountIndex]);

            // Email aus Emails löschen
            Emails.Remove(emailToDelete);

            // TODO mehrere Emails löschen funktioniert noch nicht
            //foreach(var email in SelectedEmailsToDelete)
            //{
            //    Emails.Remove(email);
            //}
        }

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !_errors.ContainsKey(propertyName)) return null;
            return _errors[propertyName];
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                if (isWarning) _errors[propertyName].Add(error);
                else _errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) &&
                _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0) _errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        public void EndEdit()
        {
            //throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            //throw new NotImplementedException();
        }
    }
}
