using System;
using System.Collections;
using System.Collections.Generic;
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
    public class SettingsController : INotifyPropertyChanged
    {
        private Account _selectedAccount;

        public ArrayList accounts { get; set; }
        public Account selectedAccount {
            get { return this._selectedAccount; }
            set { this._selectedAccount = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SettingsController()
        {
            this.accounts = new ArrayList();
            load();
        }

        public void addAccount(string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort)
        {
            Account account = new Account(user, email, password, useImap, imapPop3Server, imapPop3Port, smtpServer, smtpPort);

            accounts.Add(account);

            Console.WriteLine("selectedAccount: " + selectedAccount);

            if (selectedAccount == null)
            {
                selectedAccount = account;
                Console.WriteLine("selectedAccount: " + selectedAccount.email);
            }

            Console.WriteLine("selectedAccount: " + selectedAccount.email);

            save();
        }

        public void removeAccount(Account account)
        {
            accounts.Remove(account);

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
            formatter.Serialize(accountsStream, this.accounts);
            accountsStream.Close();

            Stream selectedAccountStream = new FileStream("selectedAccount.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(selectedAccountStream, this.selectedAccount);
            Console.WriteLine("Saved Selected Account: " + this.selectedAccount.email);
            selectedAccountStream.Close();
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
                this.accounts = (ArrayList)formatter.Deserialize(accountsStream);

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
                this.selectedAccount = (Account)formatter.Deserialize(selectedAccountStream);

                Console.WriteLine("Loaded Selected Account: " + this.selectedAccount.email);

                selectedAccountStream.Close();
            }
            catch (FileNotFoundException e)
            {
                // TODO Fehlermeldung
                Console.WriteLine("FileNotFoundException: " + e);
            }
        }
    }
}
