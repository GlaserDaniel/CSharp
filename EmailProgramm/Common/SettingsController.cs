using System;
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
        public Account account { get; set; }

        public SettingsController()
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("account.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                this.account = (Account)formatter.Deserialize(stream);
                stream.Close();
            } catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + e);
            }
        }

        public void setAccount(String user, String email, String password, String server, int port)
        {
            account = new Account(user, email, password, server, port);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("account.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, account);
            stream.Close();
        }
    }
}
