using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Model
{
    public static class DataService
    {
        private static readonly string FILE_NAME = "Accounts.xml";

        public static async Task<List<Account>> LoadAccountsAsync()
        {
            await Task.Delay(0);
            try
            {
                var fileStream = File.Open(FILE_NAME, FileMode.Open);
                var serializer = new XmlSerializer(typeof(List<Account>));
                var obj = serializer.Deserialize(fileStream);
                var accounts = obj as List<Account>;
                return accounts;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Laden: {e.Message}");
                Console.Out.WriteLine(e.StackTrace);
                return LoadSampleAccounts();
            }
        }

        public static List<Account> LoadSampleAccounts()
        {
            return new List<Account>
                {
                    new Account
                    {
                        User = "danielglasertest@gmail.com",
                        Email = "danielglasertest@gmail.com",
                        Password = "EmailTestKonto",
                        ImapPop3Server = "imap.gmail.com",
                        ImapPop3Port = 993,
                        SmtpServer = "smtp.gmail.com",
                        SmtpPort = 587,
                        UseImap = true
                    }
                };
        }

        public static async Task SaveContactsAsync(List<AccountViewModel> viewModels)
        {
            await Task.Delay(0);
            try
            {
                using (var fileStream = File.Open(FILE_NAME, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<Account>));
                    var accountList = new List<Account>();

                    viewModels.ForEach(vm =>
                    {
                        accountList.Add(new Account
                        {
                            User = vm.User,
                            Email = vm.Email,
                            Password = vm.Password,
                            ImapPop3Server = vm.ImapPop3Server,
                            ImapPop3Port = vm.ImapPop3Port,
                            SmtpServer = vm.SmtpServer,
                            SmtpPort = vm.SmtpPort,
                            UseImap = vm.UseImap
                        });
                    });
                    serializer.Serialize(fileStream, accountList);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Fehler beim Speichern: {e.Message}");
            }
        }
    }
}
