using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Account> accounts;

        public ObservableCollection<Account> Accounts
        {
            get { return accounts; }
            set {
                if (accounts == value) return;
                accounts = value;
                OnPropertyChanged("Accounts");
            }
        }

        public int selectedAccountIndex { get; set; }
        public Account selectedAccount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // protected virtual 
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SettingsViewModel()
        {
            Accounts = new ObservableCollection<Account>();
            selectedAccountIndex = -1;
            load();
        }

        public void addAccount(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort)
        {
            Account account = new Account(user, email, password, useImap, imapPop3Server, imapPop3Port, smtpServer, smtpPort);

            Accounts.Add(account);
            OnPropertyChanged("Accounts");

            Console.WriteLine("bei adden selectedAccount: " + selectedAccount + ", Index: " + selectedAccountIndex);

            if (selectedAccount == null || selectedAccountIndex == -1)
            {
                selectedAccountIndex = Accounts.IndexOf(account);
                selectedAccount = (Account)Accounts[Accounts.Count-1];
                Console.WriteLine("selectedAccount if null: " + selectedAccount.email + ", Index: " + selectedAccountIndex);
            }

            Console.WriteLine("nach adden selectedAccount: " + selectedAccount.email + ", Index: " + selectedAccountIndex);

            save();
        }

        public void removeAccount(Account accountToRemove)
        {
            Console.WriteLine("Vor löschen Accounts: ");
            foreach (Account account in this.Accounts)
            {
                Console.WriteLine("Account: " + account.ToString());
            }
            Console.WriteLine("Zu löschender Account: " + accountToRemove.ToString());

            Accounts.Remove(accountToRemove);
            OnPropertyChanged("Accounts");

            Console.WriteLine("Gelöschter Account: " + accountToRemove.ToString());
            Console.WriteLine("Nach löschen Accounts: ");
            foreach (Account account in this.Accounts)
            {
                Console.WriteLine("Account: " + account.ToString());
            }
            if (Accounts.Count == 0)
            {
                selectedAccountIndex = -1;
            }
            save();
        }

        public void appendSettings()
        {
            save();
        }

        private void save()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream accountsStream = new FileStream("accounts.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(accountsStream, Accounts);
            Console.WriteLine("Saved Accounts: ");
            foreach (Account account in Accounts)
            {
                Console.WriteLine("Account: " + account.ToString());
            }
            accountsStream.Close();
            if (selectedAccount != null)
            {
                Stream selectedAccountStream = new FileStream("selectedAccount.bin",
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(selectedAccountStream, selectedAccount);
                Console.WriteLine("Saved Selected Account: " + selectedAccount.email);
                selectedAccountStream.Close();
            }
            Stream selectedAccountIndexStream = new FileStream("selectedAccountIndex.bin",
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
            formatter.Serialize(selectedAccountIndexStream, selectedAccountIndex);
            Console.WriteLine("Saved Selected Account Index: " + selectedAccountIndex);
            selectedAccountIndexStream.Close();
        }

        private void load()
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream accountsStream = new FileStream("accounts.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                Accounts = (ObservableCollection<Account>)formatter.Deserialize(accountsStream);
                Console.WriteLine("Loaded Accounts: ");
                foreach (Account account in Accounts)
                {
                    Console.WriteLine("Account: " + account.ToString());
                }
                accountsStream.Close();

            }
            catch (FileNotFoundException e)
            {
                // TODO Fehlermeldung
                Console.WriteLine("FileNotFoundException: " + e);
            }

            try
            {
                Stream selectedAccountStream = new FileStream("selectedAccount.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                selectedAccount = (Account)formatter.Deserialize(selectedAccountStream);
                Console.WriteLine("Loaded Selected Account: " + selectedAccount.email);
                selectedAccountStream.Close();
            }
            catch (FileNotFoundException e)
            {
                // TODO Fehlermeldung
                Console.WriteLine("FileNotFoundException: " + e);
                selectedAccount = null;
            }
            catch (SerializationException se)
            {
                // TODO Fehlermeldung
                Console.WriteLine("SerializationException: " + se);
                selectedAccount = null;
            }

            try
            {
                Stream selectedAccountIndexStream = new FileStream("selectedAccountIndex.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                selectedAccountIndex = (int)formatter.Deserialize(selectedAccountIndexStream);
                Console.WriteLine("Loaded Selected Account Index: " + selectedAccountIndex);
                selectedAccountIndexStream.Close();
            }
            catch (FileNotFoundException e)
            {
                // TODO Fehlermeldung
                Console.WriteLine("FileNotFoundException: " + e);
            }
            catch (SerializationException se)
            {
                // TODO Fehlermeldung
                Console.WriteLine("SerializationException: " + se);
            }
        }
    }
}
