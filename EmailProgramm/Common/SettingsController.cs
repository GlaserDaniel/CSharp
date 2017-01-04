using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SettingsController
    {
        public ArrayList accounts { get; set; }

        public SettingsController()
        {
            accounts = new ArrayList();

            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("accounts.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                this.accounts = (ArrayList)formatter.Deserialize(stream);
                stream.Close();
            } catch (FileNotFoundException e)
            {
                // TODO Fehlermeldung
                Console.WriteLine("FileNotFoundException: " + e);
            }
        }

        public void addAccount(String user, String email, String password, String server, int port)
        {
            Account account = new Account(user, email, password, server, port);

            accounts.Add(account);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("accounts.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, accounts);
            stream.Close();
        }

        public void removeAccount(Account account)
        {
            accounts.Remove(account);
        }
    }
}
