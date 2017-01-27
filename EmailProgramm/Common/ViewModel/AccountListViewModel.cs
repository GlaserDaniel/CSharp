using Common.Exceptions;
using Common.Model;
using Common.Services;
using Common.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    /// <summary>
    /// Klasse für die Einstllungen.
    /// </summary>
    public class AccountListViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        private static AccountListViewModel instance;

        private ObservableCollection<AccountViewModel> _accounts;

        /// <summary>
        /// Der Index des ausgewählten Accounts. Falls keine ausgewählt ist, ist er -1.
        /// </summary>
        public int SelectedAccountIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private AccountListViewModel()
        {
            // TODO vielleicht unnötig
            Accounts = new ObservableCollection<AccountViewModel>();
            SelectedAccountIndex = -1;
            loadAsync();
        }

        public static AccountListViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountListViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Eine Liste aller Accounts.
        /// </summary>
        public ObservableCollection<AccountViewModel> Accounts
        {
            get { return _accounts; }
            set
            {
                if (_accounts == value) return;
                _accounts = value;
                OnPropertyChanged("Accounts");
            }
        }

        // TODO protected virtual 
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> TestIMAPServerAsync(string imapServer, string imapPort)
        {
            if (String.IsNullOrEmpty(imapServer))
            {
                throw new IMAPPOP3ServerEmptyException();
            }

            int imapPortInt = 0;

            if (String.IsNullOrEmpty(imapPort))
            {
                throw new IMAPPOP3PortEmptyException();
            }
            else
            {
                try
                {
                    imapPortInt = int.Parse(imapPort);
                }
                catch (FormatException)
                {
                    throw new IMAPPOP3PortFormatException();
                }
            }

            try
            {
                return await new EmailService().TestImapServerAsync(imapServer, imapPortInt);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
            catch (UriFormatException)
            {
                throw new UriFormatException();
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test IMAP ecxeption in AccountListViewModel: " + e);
                throw;
            }
        }

        public async Task<bool> TestPOP3ServerAsync(string pop3Server, string pop3Port)
        {
            if (String.IsNullOrEmpty(pop3Server))
            {
                throw new IMAPPOP3ServerEmptyException();
            }

            int pop3PortInt = 0;

            if (String.IsNullOrEmpty(pop3Port))
            {
                throw new IMAPPOP3PortEmptyException();
            }
            else
            {
                try
                {
                    pop3PortInt = int.Parse(pop3Port);
                }
                catch (FormatException)
                {
                    throw new IMAPPOP3PortFormatException();
                }
            }

            try
            {
                return await new EmailService().TestPop3ServerAsync(pop3Server, pop3PortInt);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
            catch (UriFormatException)
            {
                throw new UriFormatException();
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test POP3 ecxeption in AccountListViewModel: " + e);
                throw;
            }
        }

        public async Task<bool> TestSMTPServerAsync(string smtpServer, string smtpPort)
        {
            if (String.IsNullOrEmpty(smtpServer))
            {
                throw new SMTPServerEmptyException();
            }

            int smtpPortInt = 0;

            if (String.IsNullOrEmpty(smtpPort))
            {
                throw new SMTPPortEmtpyException();
            }
            else
            {
                try
                {
                    smtpPortInt = int.Parse(smtpPort);
                }
                catch (FormatException)
                {
                    throw new SMTPPortFormatException();
                }
            }

            try
            {
                return await new EmailService().TestSmtpServerAsync(smtpServer, smtpPortInt);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
            catch (UriFormatException)
            {
                throw new UriFormatException();
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test SMTP ecxeption in AccountListViewModel: " + e);
                throw;
            }
        }

        public void addAccount(AccountViewModel account)
        {
            Accounts.Add(account);

            if (SelectedAccountIndex == -1)
            {
                SelectedAccountIndex = Accounts.IndexOf(account);
            }
        }

        public void addAccount(string showname, string user, string email, string password, bool useImap, string imapPop3Server, string imapPop3Port, string smtpServer, string smtpPort)
        {
            if (String.IsNullOrEmpty(showname))
            {
                throw new ShownameEmptyException();
            }

            if (String.IsNullOrEmpty(user))
            {
                throw new UserEmptyException();
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new EmailEmptyException();
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new PasswordEmptyException();
            }

            if (String.IsNullOrEmpty(imapPop3Server))
            {
                throw new IMAPPOP3ServerEmptyException();
            }

            int imapPop3PortInt = 0;

            if (String.IsNullOrEmpty(imapPop3Port))
            {
                throw new IMAPPOP3PortEmptyException();
            }
            else
            {
                try
                {
                    imapPop3PortInt = int.Parse(imapPop3Port);
                }
                catch (FormatException)
                {
                    throw new IMAPPOP3PortFormatException();
                }
            }

            if (String.IsNullOrEmpty(smtpServer))
            {
                throw new SMTPServerEmptyException();
            }

            int smtpPortInt = 0;

            if (String.IsNullOrEmpty(smtpPort))
            {
                throw new SMTPPortEmtpyException();
            }
            else
            {
                try
                {
                    smtpPortInt = int.Parse(smtpPort);
                }
                catch (FormatException)
                {
                    throw new SMTPPortFormatException();
                }
            }

            AccountViewModel account = new AccountViewModel(showname, user, email, password, useImap, imapPop3Server, imapPop3PortInt, smtpServer, smtpPortInt, new ObservableCollection<EmailViewModel>());

            Accounts.Add(account);

            if (SelectedAccountIndex == -1) //selectedAccount == null || 
            {
                SelectedAccountIndex = Accounts.IndexOf(account);
            }

            //saveAsync();
        }

        public void removeAccount(AccountViewModel accountToRemove)
        {
            //Console.WriteLine("Vor löschen Accounts: ");
            //foreach (Account account in this.Accounts)
            //{
            //    Console.WriteLine("Account: " + account.ToString());
            //}
            //Console.WriteLine("Zu löschender Account: " + accountToRemove.ToString());

            Accounts.Remove(accountToRemove);
            //OnPropertyChanged("Accounts");

            //Console.WriteLine("Gelöschter Account: " + accountToRemove.ToString());
            //Console.WriteLine("Nach löschen Accounts: ");
            //foreach (Account account in this.Accounts)
            //{
            //    Console.WriteLine("Account: " + account.ToString());
            //}
            if (Accounts.Count == 0)
            {
                SelectedAccountIndex = -1;
            }
            saveAsync();
        }

        public async void saveAsync()
        {
            Console.WriteLine("save");
            await DataService.SaveAccountsAsync(Accounts.ToList());

            await DataService.SaveSelectedAccountIndexAsync(SelectedAccountIndex);

            //IFormatter formatter = new BinaryFormatter();
            //Stream accountsStream = new FileStream("accounts.bin",
            //                         FileMode.Create,
            //                         FileAccess.Write, FileShare.None);
            //formatter.Serialize(accountsStream, Accounts); // TODO handle SerilazionException wegen PropertyChanged not serilizble
            ////Console.WriteLine("Saved Accounts: ");
            ////foreach (Account account in Accounts)
            ////{
            ////    Console.WriteLine("Account: " + account.ToString());
            ////}
            //accountsStream.Close();

            //if (selectedAccount != null)
            //{
            //    Stream selectedAccountStream = new FileStream("selectedAccount.bin",
            //                             FileMode.Create,
            //                             FileAccess.Write, FileShare.None);
            //    formatter.Serialize(selectedAccountStream, selectedAccount);
            //    //Console.WriteLine("Saved Selected Account: " + selectedAccount.email);
            //    selectedAccountStream.Close();
            //}

            //Stream selectedAccountIndexStream = new FileStream("selectedAccountIndex.bin",
            //                             FileMode.Create,
            //                             FileAccess.Write, FileShare.None);
            //formatter.Serialize(selectedAccountIndexStream, selectedAccountIndex);
            //Console.WriteLine("Saved Selected Account Index: " + selectedAccountIndex);
            //selectedAccountIndexStream.Close();
        }

        private async void loadAsync()
        {
            Console.WriteLine("load");
            List<Account> accountsList = await DataService.LoadAccountsAsync();

            Accounts = new ObservableCollection<AccountViewModel>();

            foreach (var account in accountsList)
            {
                Accounts.Add(new AccountViewModel(account));
            }

            SelectedAccountIndex = await DataService.LoadSelectedAccountIndexAsync();

            if (Accounts.Count == 0)
            {
                SelectedAccountIndex = -1;
            }

            //IFormatter formatter = new BinaryFormatter();
            //try
            //{
            //    Stream accountsStream = new FileStream("accounts.bin",
            //                              FileMode.Open,
            //                              FileAccess.Read,
            //                              FileShare.Read);
            //    Accounts = (ObservableCollection<AccountViewModel>)formatter.Deserialize(accountsStream);
            //    //Console.WriteLine("Loaded Accounts: ");
            //    //foreach (Account account in Accounts)
            //    //{
            //    //    Console.WriteLine("Account: " + account.ToString());
            //    //}
            //    accountsStream.Close();
            //}
            //catch (FileNotFoundException e)
            //{
            //    // TODO Fehlermeldung
            //    Console.WriteLine("FileNotFoundException: " + e);
            //}
            //catch (SerializationException se)
            //{
            //    // TODO Fehlermeldung
            //    // Datei leer
            //    Console.WriteLine("SerializationException: " + se);
            //    Accounts = new ObservableCollection<AccountViewModel>();
            //    selectedAccountIndex = -1;
            //    return;
            //}

            //try
            //{
            //    Stream selectedAccountStream = new FileStream("selectedAccount.bin",
            //                              FileMode.Open,
            //                              FileAccess.Read,
            //                              FileShare.Read);
            //    selectedAccount = (Account)formatter.Deserialize(selectedAccountStream);
            //    //Console.WriteLine("Loaded Selected Account: " + selectedAccount.email);
            //    selectedAccountStream.Close();
            //}
            //catch (FileNotFoundException e)
            //{
            //    // TODO Fehlermeldung
            //    Console.WriteLine("FileNotFoundException: " + e);
            //    selectedAccount = null;
            //}
            //catch (SerializationException se)
            //{
            //    // TODO Fehlermeldung
            //    // Datei leer
            //    Console.WriteLine("SerializationException: " + se);
            //    selectedAccount = null;
            //}

            //try
            //{
            //    Stream selectedAccountIndexStream = new FileStream("selectedAccountIndex.bin",
            //                              FileMode.Open,
            //                              FileAccess.Read,
            //                              FileShare.Read);
            //    selectedAccountIndex = (int)formatter.Deserialize(selectedAccountIndexStream);
            //    //Console.WriteLine("Loaded Selected Account Index: " + selectedAccountIndex);
            //    selectedAccountIndexStream.Close();
            //}
            //catch (FileNotFoundException e)
            //{
            //    // TODO Fehlermeldung
            //    Console.WriteLine("FileNotFoundException: " + e);
            //}
            //catch (SerializationException se)
            //{
            //    // TODO Fehlermeldung
            //    // Datei leer
            //    Console.WriteLine("SerializationException: " + se);
            //    selectedAccountIndex = -1;
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


        //private bool IsImapPop3PortNotEmpty(String imapPop3Port)
        //{
        //    var error = "Bitte geben Sie einen Namen ein.";
        //    if (String.IsNullOrEmpty(imapPop3Port))
        //    {
        //        AddError(nameof(ImapPop3Port), error, false);
        //        return false;
        //    }
        //    RemoveError(nameof(ImapPop3Port), error);
        //    return true;
        //}
    }
}
