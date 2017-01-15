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
using MimeKit;
using System.ComponentModel;
using System.Collections;

namespace Common
{
    /// <summary>
    /// Klasse um alles mit Emails machen zu können.
    /// </summary>
    public class EmailViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public EmailViewModel()
        {

        }

        public Task<bool> TestImapServer(String imapServer, int imapPort)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (ImapClient imapClient = new ImapClient())
                    {
                        imapClient.Timeout = 5000;
                        imapClient.Connect(imapServer, imapPort, true);
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

        public Task<bool> TestPop3Server(String pop3Server, int pop3Port)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (Pop3Client pop3Client = new Pop3Client())
                    {
                        pop3Client.Timeout = 5000;
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
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        tcpClient.ReceiveTimeout = 5000; // TODO Timeout funktioniert nicht.
                        tcpClient.Connect(smtpServer, smtpPort); // TODO Reicht nicht aus. Da wenn TLS/STARTTLS erwünscht ist der eine Port auch falsch sein kann.
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

        /// <summary>
        /// Email senden
        /// </summary>
        /// <param name="senderAccount"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        // Snippet von www.code-bude.net
        // https://code-bude.net/2011/06/14/emails-versenden-in-csharp/
        public void sendEmail(Account senderAccount, string receiver, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();

            //Absender konfigurieren
            mailMessage.From = new MailAddress(senderAccount.Email);

            //Empfänger konfigurieren
            mailMessage.To.Add(new MailAddress(receiver.Trim()));

            //Betreff einrichten
            mailMessage.Subject = subject;

            //Hinzufügen der eigentlichen Nachricht
            mailMessage.Body = message;

            //Ausgangsserver initialisieren
            SmtpClient smtpClient = new SmtpClient(senderAccount.SmtpServer, senderAccount.SmtpPort);

            // SSL aktivieren
            // Scheint unnötig da es sowieso gesetzt wird (funktioniert auch ohne)
            smtpClient.EnableSsl = true;

            if (senderAccount.User.Length > 0 && senderAccount.User != string.Empty)
            {
                //Login konfigurieren
                smtpClient.Credentials = new System.Net.NetworkCredential(senderAccount.User, senderAccount.Password);
                Console.WriteLine("Password: " + senderAccount.Password);
            }

            Console.WriteLine(mailMessage.To);

            //Email absenden
            try
            {
                smtpClient.Send(mailMessage); // TODO SendMailAsync
                Console.WriteLine("Email gesendet!");
            }
            catch (SmtpException e)
            {
                // TODO Benachrichtigung das es nicht funktioniert hat!
                Console.WriteLine("SMTP Exception: " + e);
            }
        }

        /// <summary>
        /// Emails abholen
        /// </summary>
        /// <param name="account"></param>
        /// <param name="progressHandler"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public Task receiveEmails(Account account, Progress<double> progressHandler, Dispatcher dispatcher)
        {
            return Task.Run(() =>
            {
                var progress = progressHandler as IProgress<double>;

                if (account != null)
                {
                    if (account.UseImap)
                    {
                        try
                        {
                            using (ImapClient imapClient = new ImapClient())
                            {
                                imapClient.Connect(account.ImapPop3Server, account.ImapPop3Port, true);

                                imapClient.Authenticate(account.User, account.Password);

                                imapClient.Inbox.Open(FolderAccess.ReadOnly);

                                var uids = imapClient.Inbox.Search(SearchQuery.All); // vorher New

                                // TODO entfernen
                                Console.WriteLine("Emails IMAP Anzahl: " + uids.Count);

                                foreach (var uid in uids)
                                {
                                    Console.WriteLine("uid: " + uid.Id);
                                    if (progress != null)
                                    {
                                        progress.Report((uids.IndexOf(uid) * 100) / uids.Count);
                                    }

                                    if (!account.doEmailsContainsId((int)uid.Id))
                                    {
                                        // Nachricht holen
                                        var message = imapClient.Inbox.GetMessage(uid);

                                        // write the message to a file
                                        //message.WriteTo(string.Format("{0}.msg", uid));

                                        Console.WriteLine("Message " + uid + ": " + message.Subject);

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

                                        email.id = (int)uid.Id;

                                        var currentAccount = account;
                                        var currentEmail = email;

                                        dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            currentAccount.Emails.Add(currentEmail);
                                        }));
                                    }
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
                                client.Connect(account.ImapPop3Server, account.ImapPop3Port, true);

                                client.Authenticate(account.User, account.Password);

                                //var uids = client.GetMessageUids();

                                //foreach (var uid in uids)
                                //{
                                //    client.GetMessage(uid);
                                //}

                                // TODO Erstmal nur 50 Emails abholen
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

                                    HeaderList list = client.GetMessageHeaders(i);

                                    foreach (var header in list)
                                    {
                                        Console.WriteLine("headerID: " + header.Id);
                                        Console.WriteLine("header Value: " + header.Value);
                                    }

                                    if (!account.doEmailsContainsId(i))
                                    {
                                        // Nachricht holen
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

                                        email.id = i;

                                        var currentAccount = account;
                                        var currentEmail = email;

                                        dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            currentAccount.Emails.Add(currentEmail);
                                        }));

                                        // mark the message for deletion
                                        //client.DeleteMessage(i);
                                    }
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

                    if (progress != null)
                    {
                        progress.Report(100);
                    }

                    // Warte 5 Sekunden
                    Thread.Sleep(5000);

                    // Und setze die ProgressBar wieder auf 0
                    if (progress != null)
                    {
                        progress.Report(0);
                    }

                    // TODO Testausgabe
                    //Console.WriteLine("Emails ausgeben: ");
                    //foreach (Account account in settingsViewModel.Accounts)
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

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
    }
}
