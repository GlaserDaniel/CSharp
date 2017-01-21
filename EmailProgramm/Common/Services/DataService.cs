using Common.Model;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Services
{
    public static class DataService
    {
        private static readonly string ACCOUNTS_FILE_NAME = "Accounts.xml";

        private static readonly string SELECTED_ACCOUNT_INDEX_FILE_NAME = "SelectedAccountIndex.xml";

        public static async Task<List<Account>> LoadAccountsAsync()
        {
            await Task.Delay(0);
            try
            {
                var fileStream = File.Open(ACCOUNTS_FILE_NAME, FileMode.Open);
                var serializer = new XmlSerializer(typeof(List<Account>));
                var obj = serializer.Deserialize(fileStream);
                var accounts = obj as List<Account>;
                return accounts;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Laden von Accounts: {e.Message}");
                Console.Out.WriteLine(e.StackTrace); // TODO Error Message Box für Nutzer das Beispiel Daten geladen wurden.
                return LoadSampleAccounts();
            }
        }

        public static List<Account> LoadSampleAccounts()
        {
            return new List<Account>
                {
                    new Account
                    {
                        Showname = "Test Account IMAP",
                        User = "danielglasertest@gmail.com",
                        Email = "danielglasertest@gmail.com",
                        Password = "EmailTestKonto",
                        ImapPop3Server = "imap.gmail.com",
                        ImapPop3Port = 993,
                        SmtpServer = "smtp.gmail.com",
                        SmtpPort = 587,
                        UseImap = true // TODO Email new emty List<Email>()
                    }
                };
        }

        public static async Task SaveAccountsAsync(List<AccountViewModel> viewModels)
        {
            await Task.Delay(0);
            try
            {
                using (var fileStream = File.Open(ACCOUNTS_FILE_NAME, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<Account>));
                    var accountList = new List<Account>();

                    viewModels.ForEach(vm =>
                    {
                        accountList.Add(new Account
                        {
                            Showname = vm.Showname,
                            User = vm.User,
                            Email = vm.Email,
                            Password = vm.Password,
                            ImapPop3Server = vm.ImapPop3Server,
                            ImapPop3Port = vm.ImapPop3Port,
                            SmtpServer = vm.SmtpServer,
                            SmtpPort = vm.SmtpPort,
                            UseImap = vm.UseImap,
                            // TODO
                            //Emails = vm.Emails.ToList()
                            //TODO
                            //EmailViewModel emailsVM = new EmailViewModel(account.Emails);
                            //Emails = new ObservableCollection<EmailViewModel>(emailsVM);
                        });
                    });
                    serializer.Serialize(fileStream, accountList);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Speichern von Accounts: {e.Message}");
            }
        }

        public static async Task<int> LoadSelectedAccountIndexAsync()
        {
            await Task.Delay(0);
            try
            {
                var fileStream = File.Open(SELECTED_ACCOUNT_INDEX_FILE_NAME, FileMode.Open);
                var serializer = new XmlSerializer(typeof(int));
                var obj = serializer.Deserialize(fileStream);
                var selectedAccountIndex = (int)obj;
                return selectedAccountIndex;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Laden von SelectedAccountIndex: {e.Message}");
                Console.Out.WriteLine(e.StackTrace);
                return -1;
            }
        }

        public static async Task SaveSelectedAccountIndexAsync(int selectedAccountIndex)
        {
            await Task.Delay(0);
            try
            {
                using (var fileStream = File.Open(SELECTED_ACCOUNT_INDEX_FILE_NAME, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(int));
                    int selectedAccountIndexCopie = -1;

                    selectedAccountIndexCopie = selectedAccountIndex;

                    serializer.Serialize(fileStream, selectedAccountIndexCopie);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Speichern von SelectedAccountIndex: {e.Message}");
            }
        }
    }
}
