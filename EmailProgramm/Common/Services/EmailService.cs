using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.IO;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading;
using MimeKit;
using System.ComponentModel;
using System.Collections;
using Common.ViewModel;
using System.Net.Mail;

namespace Common.Services
{
    /// <summary>
    /// Klasse um alles mit Emails machen zu können.
    /// </summary>
    public class EmailService : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public EmailService()
        {

        }

        public Task<bool> TestImapServer(string imapServer, int imapPort)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (ImapClient imapClient = new ImapClient())
                    {
                        imapClient.Timeout = 1000;
                        Task ConnectTask = imapClient.ConnectAsync(imapServer, imapPort, true);

                        ConnectTask.Wait(1000);

                        if (imapClient.IsConnected)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> TestPop3Server(string pop3Server, int pop3Port)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (Pop3Client pop3Client = new Pop3Client())
                    {
                        pop3Client.Timeout = 1000;
                        Task ConnectTask = pop3Client.ConnectAsync(pop3Server, pop3Port, true);

                        ConnectTask.Wait(1000);

                        if (pop3Client.IsConnected)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> TestSmtpServer(string smtpServer, int smtpPort)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        smtpClient.Timeout = 1000;

                        Task ConnectTask = smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                        ConnectTask.Wait(1000);

                        if (smtpClient.IsConnected)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
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
        // und http://www.mimekit.net/docs/html/T_MailKit_Net_Smtp_SmtpClient.htm kombiniert und abgeändert
        public void sendEmail(AccountViewModel senderAccount, List<string> receivers, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();

            //Absender konfigurieren
            mailMessage.From = new MailAddress(senderAccount.Email);

            //Empfänger konfigurieren
            foreach (var receiver in receivers)
            {
                mailMessage.To.Add(new MailAddress(receiver.Trim()));
            }

            //Betreff einrichten
            mailMessage.Subject = subject;

            //Hinzufügen der eigentlichen Nachricht
            mailMessage.Body = message;

            //Ausgangsserver initialisieren
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(senderAccount.SmtpServer, senderAccount.SmtpPort);

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
                smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email gesendet!");
            }
            catch (SmtpException e)
            {
                // TODO Benachrichtigung das es nicht funktioniert hat!
                Console.WriteLine("SMTP Exception: " + e);
            }

            //MailKit SMTPClient hat noch nicht geklappt
            //MimeMessage mailMessage = new MimeMessage();

            ////Absender konfigurieren
            //mailMessage.Sender = new MailboxAddress(senderAccount.Email.ToString());

            ////Empfänger konfigurieren
            //foreach (var receiver in receivers)
            //{
            //    mailMessage.To.Add(new MailboxAddress(receiver.Trim())); // Das scheint nicht zu funktionieren
            //}

            ////Betreff einrichten
            //mailMessage.Subject = subject;

            ////Hinzufügen der eigentlichen Nachricht
            //mailMessage.Body = new TextPart(message);

            ////Ausgangsserver initialisieren
            //MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();
            //smtpClient.Connect(senderAccount.SmtpServer, senderAccount.SmtpPort);

            //if (senderAccount.User.Length > 0 && senderAccount.User != string.Empty)
            //{
            //    //Login konfigurieren
            //    smtpClient.Authenticate(senderAccount.User, senderAccount.Password);
            //}

            //Console.WriteLine(mailMessage.To);

            ////Email absenden
            //try
            //{
            //    smtpClient.SendAsync(mailMessage);
            //    Console.WriteLine("Email gesendet!");
            //}
            //catch (Exception e)
            //{
            //    // TODO Benachrichtigung das es nicht funktioniert hat!
            //    Console.WriteLine("SMTP Exception: " + e);
            //}
        }

        /// <summary>
        /// Emails abholen
        /// </summary>
        /// <param name="account"></param>
        /// <param name="progressHandler"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public Task receiveEmails(AccountViewModel account, Progress<double> progressHandler, Dispatcher dispatcher)
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

                                var uids = imapClient.Inbox.Search(SearchQuery.All);

                                foreach (UniqueId uid in uids)
                                {
                                    Console.WriteLine("uid: " + uid.Id);
                                    if (progress != null)
                                    {
                                        progress.Report((uids.IndexOf(uid) * 100) / uids.Count);
                                    }

                                    if (!account.doEmailsContainsId((int)uid.Id))
                                    {
                                        // Nachricht holen
                                        MimeMessage message = imapClient.Inbox.GetMessage(uid);

                                        Console.WriteLine("MessageID: " + message.MessageId);

                                        // write the message to a file
                                        //message.WriteTo(string.Format("{0}.msg", uid));

                                        convertMessageToEmail(message, (int)uid.Id, account, dispatcher);
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

                                int count = client.Count;

                                // TODO Erstmal nur 50 Emails abholen
                                //if (client.Count > 50)
                                //{
                                //    count = 50;
                                //}

                                for (int id = 0; id < count; id++)
                                {
                                    if (progress != null)
                                    {
                                        progress.Report((id * 100) / count);
                                    }

                                    HeaderList list = client.GetMessageHeaders(id);

                                    foreach (var header in list)
                                    {
                                        Console.WriteLine("headerID: " + header.Id);
                                        Console.WriteLine("header Value: " + header.Value);
                                    }

                                    if (!account.doEmailsContainsId(id))
                                    {
                                        // Nachricht holen
                                        var message = client.GetMessage(id);

                                        Console.WriteLine("MessageID: " + message.MessageId);

                                        // write the message to a file
                                        //message.WriteTo(string.Format("{0}.msg", i));

                                        convertMessageToEmail(message, id, account, dispatcher);
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

                    // Damit weil gespeichert wird und im Hintergrund die ProgressBar zurückgestzt wird.
                    Task.Run(() =>
                    {
                        // Warte 2 Sekunden
                        Thread.Sleep(2000);

                        // Und setze die ProgressBar wieder auf 0
                        if (progress != null)
                        {
                            progress.Report(0);
                        }
                    });

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

        private void convertMessageToEmail(MimeMessage message, int id, AccountViewModel account, Dispatcher dispatcher)
        {
            EmailViewModel email = new EmailViewModel();

            email.Sender = message.From.ToString();

            foreach (MimeKit.InternetAddress inetAdress in message.To)
            {
                email.Receivers.Add(inetAdress.ToString());
            }

            if (!String.IsNullOrEmpty(message.Subject))
            {
                email.Subject = message.Subject.ToString();
            }
            else
            {
                email.Subject = "(Kein Betreff)";
            }

            if (!string.IsNullOrEmpty(message.HtmlBody))
            {
                email.Message = message.HtmlBody.ToString();
                email.IsHtml = true;
            }
            else
            {
                //email.Message = message.Body.ToString();
                if (message.TextBody != null)
                {
                    email.Message = message.TextBody.ToString();
                }
                else
                {
                    email.Message = ""; // kein Body
                }
                email.IsHtml = false;
            }

            email.DateTime = message.Date.DateTime;

            email.Id = id;

            var currentAccount = account;
            var currentEmail = email;

            dispatcher.BeginInvoke((Action)(() =>
            {
                currentAccount.Emails.Add(currentEmail);
            }));
        }

        public void deleteMessage(EmailViewModel email, AccountViewModel account)
        {
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

                            imapClient.Inbox.Open(FolderAccess.ReadWrite);

                            // zum Löschen markieren
                            // TODO funktioniert noch nicht
                            imapClient.Inbox.AddFlags(email.Id, MessageFlags.Deleted, false);

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

                            // zum Löschen markieren
                            client.DeleteMessage(email.Id);

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
            }
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
