using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.IO;
using MailKit.Net.Pop3;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading;

namespace Common
{
    public class EmailViewModel
    {
        // TODO entfernen
        Account account;

        public EmailViewModel()
        {
            
        }

        public EmailViewModel(Account account)
        {
            this.account = account;
        }

        public Task<bool> TestImapServer(String imapServer, int imapPort)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (ImapClient imapClient = new ImapClient())
                    {
                        imapClient.Connect(imapServer, imapPort, true);
                    }
                }
                catch (IOException)
                {
                    return false;
                }
                return true;
            });
        }

        public Task<bool> TestPop3Server(String pop3Server, int pop3Port)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (Pop3Client pop3Client = new Pop3Client())
                    {
                        pop3Client.Connect(pop3Server, pop3Port, true);
                    }
                }
                catch (IOException)
                {
                    return false;
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            });
        }

        public Task<bool> TestSmtpServer(String smtpServer, int smtpPort)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        //smtpClient.(smtpServer, smtpPort, true);
                    }
                }
                catch (IOException)
                {
                    return false;
                }
                return true;
            });
        }

        // Snippet von www.code-bude.net
        // https://code-bude.net/2011/06/14/emails-versenden-in-csharp/
        public void sendEmail(string sender, string receiver, string subject, string message)
        {
            SettingsViewModel settingsController = new SettingsViewModel();

            // wenn ein Account als Standard definiert/ausgewählt wurde
            if (settingsController.selectedAccountIndex != -1)
            {
                // TODO anhand der ausgewähten EMail Adresse Account nutzen
                account = (Account)settingsController.Accounts[settingsController.selectedAccountIndex];

                MailMessage mailMessage = new MailMessage();

                //Absender konfigurieren
                mailMessage.From = new MailAddress(sender);

                //Empfänger konfigurieren
                mailMessage.To.Add(new MailAddress(receiver.Trim()));

                //Betreff einrichten
                mailMessage.Subject = subject;

                //Hinzufügen der eigentlichen Nachricht
                mailMessage.Body = message;

                //Ausgangsserver initialisieren
                SmtpClient smtpClient = new SmtpClient(account.smtpServer, account.smtpPort);

                // SSL aktivieren
                // Scheint unnötig da es sowieso gesetzt wird (funktioniert auch ohne)
                smtpClient.EnableSsl = true;

                if (account.user.Length > 0 && account.user != string.Empty)
                {
                    //Login konfigurieren
                    smtpClient.Credentials = new System.Net.NetworkCredential(account.user, account.password);
                    Console.WriteLine("Password: " + account.password);
                }

                Console.WriteLine(mailMessage.To);

                //Email absenden
                try
                {
                    smtpClient.SendMailAsync(mailMessage); // TODO SendMailAsync
                }
                catch (SmtpException e)
                {
                    // TODO Benachrichtigung das es nicht funktioniert hat!
                    Console.WriteLine("SMTP Exception: " + e);
                }
            }
        }

        public Task receiveEmails(Account account, Progress<double> progressHandler, Dispatcher dispatcher)
        {
            return Task.Run(() => {
                this.account = account;
                var progress = progressHandler as IProgress<double>;

                SettingsViewModel settingsController = new SettingsViewModel();

                if (account != null)
                {
                    if (account.useImap)
                    {
                        try
                        {
                            using (ImapClient imapClient = new ImapClient())
                            {
                                imapClient.Connect(account.smtpServer, account.smtpPort, true);

                                imapClient.Authenticate(account.user, account.password);

                                imapClient.Inbox.Open(FolderAccess.ReadOnly);

                                var uids = imapClient.Inbox.Search(SearchQuery.New);

                                foreach (var uid in uids)
                                {
                                    var message = imapClient.Inbox.GetMessage(uid);

                                    // write the message to a file
                                    //message.WriteTo(string.Format("{0}.msg", uid));

                                    Console.WriteLine("Message " + uid + ": " + message.Subject);
                                }

                                imapClient.Disconnect(true);
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("IOException: " + e);
                        }
                        catch (SocketException se)
                        {
                            Console.WriteLine("SocketException: " + se);
                        }
                    }
                    else
                    {
                        try
                        {
                            using (var client = new Pop3Client())
                            {
                                client.Connect(account.imapPop3Server, account.imapPop3Port, true);

                                client.Authenticate(account.user, account.password);

                                // TODO Erstmal nur 10 Emails abholen
                                int count = client.Count;
                                if (client.Count > 50)
                                {
                                    count = 50;
                                }

                                for (int i = 0; i < count; i++)
                                {
                                    if (progress != null)
                                    {
                                        progress.Report((i * 100) / count);
                                    }

                                    var message = client.GetMessage(i);

                                    // write the message to a file
                                    //message.WriteTo(string.Format("{0}.msg", i));

                                    //Console.WriteLine("Message " + i + ": " + message.Subject);

                                    Email email = new Email();

                                    email.sender = message.From.ToString();

                                    foreach (MimeKit.InternetAddress inetAdress in message.To)
                                    {
                                        email.receiver.Add(inetAdress.ToString());
                                    }

                                    email.subject = message.Subject.ToString();

                                    if (!string.IsNullOrEmpty(message.HtmlBody))
                                    {
                                        email.message = message.HtmlBody.ToString();
                                        // TODO HTML to true setzen
                                    }
                                    else
                                    {
                                        email.message = message.Body.ToString();
                                        // TODO HTML to false setzen
                                    }

                                    email.dateTime = message.Date.Date;

                                    var currentAccount = account;
                                    var currentEmail = email;

                                    dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        currentAccount.Emails.Add(currentEmail);
                                        Console.WriteLine("Im Dispatcher");
                                    }));

                                    // mark the message for deletion
                                    //client.DeleteMessage(i);
                                }

                                client.Disconnect(true);
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("IOException: " + e);
                        }
                        catch (SocketException se)
                        {
                            Console.WriteLine("SocketException: " + se);
                        }
                    }
                    settingsController.appendSettings();

                    if (progress != null)
                    {
                        progress.Report(100);
                    }

                    Thread.Sleep(5000);

                    if (progress != null)
                    {
                        progress.Report(0);
                    }

                    // TODO Testausgabe
                    //Console.WriteLine("Emails ausgeben: ");
                    //foreach (Account account in settingsController.Accounts)
                    //{
                    //    Console.WriteLine("Account: " + account);
                    //    foreach (Email email in account.Emails)
                    //    {
                    //        Console.WriteLine(email);
                    //    }
                    //}
                }
            });
        }
    }
}
