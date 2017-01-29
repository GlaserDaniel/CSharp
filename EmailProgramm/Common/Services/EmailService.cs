using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.IO;
using MailKit.Net.Pop3;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading;
using MimeKit;
using Common.ViewModel;
using System.Net.Mail;

namespace Common.Services
{
    /// <summary>
    /// Klasse mit der alles rund um E-Mails gehandhabt wird.
    /// E-Mails schicken, E-Mails abholen, löschen usw.
    /// </summary>
    public class EmailService
    {
        public EmailService()
        {

        }

        /// <summary>
        /// Zum testen des IMAP-Servers.
        /// </summary>
        /// <param name="imapServer"></param>
        /// <param name="imapPort"></param>
        /// <returns></returns>
        public async Task<bool> TestImapServerAsync(string imapServer, int imapPort)
        {
            try
            {
                using (ImapClient imapClient = new ImapClient())
                {
                    imapClient.Timeout = 1000;

                    await imapClient.ConnectAsync(imapServer, imapPort, true);

                    if (imapClient.IsConnected)
                    {
                        return true;
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
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
                Console.WriteLine("Test IMAP ecxeption: " + e);
                throw;
            }
        }

        /// <summary>
        /// Zum testen des POP3-Servers
        /// </summary>
        /// <param name="pop3Server"></param>
        /// <param name="pop3Port"></param>
        /// <returns></returns>
        public async Task<bool> TestPop3ServerAsync(string pop3Server, int pop3Port)
        {
            try
            {
                using (Pop3Client pop3Client = new Pop3Client())
                {
                    pop3Client.Timeout = 1000;
                    await pop3Client.ConnectAsync(pop3Server, pop3Port, true);

                    if (pop3Client.IsConnected)
                    {
                        return true;
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
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
                Console.WriteLine("Test POP3 ecxeption: " + e);
                throw;
            }
        }

        /// <summary>
        /// Zum testen des SMTP-Servers
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <returns></returns>
        public async Task<bool> TestSmtpServerAsync(string smtpServer, int smtpPort)
        {
            try
            {
                using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtpClient.Timeout = 1000;

                    // Das Testen des SMTP Server funktioniert noch nicht ganz so wie gewollt.
                    // Es wird im Moment getestet ob eine TLS Verschlüsselung verfügbar ist. 
                    // Falls aber ein Email Anbieter auch nur SSL genügt, würde er bei diesem auch auf den Schluss kommen 
                    // es würde nicht gehen. Wenn wiederrum auch SSL beim Testen ermöglicht wird kommen bei den Anbietern 
                    // wo TLS Pflicht ist eine Meldung es würde gehen obwohl es dann nicht funktionieren würde. 
                    // Da aber so gut wie alle Anbieter mittlerweile auf TLS setzen habe ich mich dafür entschieden.
                    await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                    if (smtpClient.IsConnected)
                    {
                        return true;
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
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
                Console.WriteLine("Test SMTP ecxeption: " + e);
                throw;
            }
        }

        /// <summary>
        /// Zum Senden einer E-Mail. (Die Uhrzeit passt noch nicht wann sie gesendet wurde.)
        /// </summary>
        /// <param name="senderAccount"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        // Snippet von www.code-bude.net
        // https://code-bude.net/2011/06/14/emails-versenden-in-csharp/
        // und http://www.mimekit.net/docs/html/T_MailKit_Net_Smtp_SmtpClient.htm kombiniert und abgeändert
        public async void sendEmailAsync(AccountViewModel senderAccount, List<string> receivers, List<string> ccs, List<string> bccs, string subject, string message, List<string> attachments)
        {
            MailMessage mailMessage = new MailMessage();

            //Absender konfigurieren
            try
            {
                mailMessage.From = new MailAddress(senderAccount.Email);
            }
            catch (FormatException)
            {
                throw new FormatException();
            }

            //Empfänger konfigurieren
            try
            {
                foreach (var receiver in receivers)
                {
                    mailMessage.To.Add(new MailAddress(receiver.Trim()));
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }

            // CC setzen
            try
            {
                foreach (var cc in ccs)
                {
                    mailMessage.CC.Add(new MailAddress(cc.Trim()));
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }

            // BCC setzen
            try
            {
                foreach (var bcc in bccs)
                {
                    mailMessage.Bcc.Add(new MailAddress(bcc.Trim()));
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }

            //Betreff einrichten
            mailMessage.Subject = subject;

            //Hinzufügen der eigentlichen Nachricht
            mailMessage.Body = message;

            // Anhang hinzufügen
            foreach (string attachment in attachments)
            {
                mailMessage.Attachments.Add(new Attachment(attachment));
            }

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
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email gesendet!");
            }
            catch (SmtpException e)
            {
                // TODO Benachrichtigung das es nicht funktioniert hat!
                Console.WriteLine("SMTP Exception: " + e);
                throw new Exception();
            }
        }

        /// <summary>
        /// Zum Abholen von E-Mails vom übergebenen Account. Speichert die E-Mails in dem übergebenen Account.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="progressHandler"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        /// von http://www.mimekit.net/docs/html/T_MailKit_Net_Imap_ImapClient.htm
        /// und http://www.mimekit.net/docs/html/T_MailKit_Net_Pop3_Pop3Client.htm erweitert und kombiniert
        public Task receiveEmails(AccountViewModel account, Progress<double> progressHandler, Dispatcher dispatcher)
        {
            try
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

                                            //Console.WriteLine("MessageID: " + message.MessageId);

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
                            catch (ImapProtocolException ipe)
                            {
                                Console.WriteLine("ImapProtocolException: " + ipe);
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
                                            //Console.WriteLine("headerID: " + header.Id);
                                            //Console.WriteLine("header Value: " + header.Value);
                                        }

                                        if (!account.doEmailsContainsId(id))
                                        {
                                            // Nachricht holen
                                            var message = client.GetMessage(id);

                                            //Console.WriteLine("MessageID: " + message.MessageId);

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
                            catch (Pop3ProtocolException ppe)
                            {
                                Console.WriteLine("Pop3ProtocolException: " + ppe);
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
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Wandelt eine MimeMessage in eine Email um und speichert sie im übergebenen Account
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="dispatcher"></param>
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

            // TODO
            //email.IsRead = message.

            email.AccountIndex = AccountListViewModel.Instance.Accounts.IndexOf(account);

            List<MimeEntity> test = message.Attachments.ToList();

            // von http://stackoverflow.com/questions/36227622/mailkit-save-attachments und abgeändert
            // [
            foreach (MimeEntity attachment in message.Attachments)
            {
                if (attachment.IsAttachment)
                {
                    if (attachment is MessagePart)
                    {
                        // nichts tun
                    }
                    else
                    {
                        var part = (MimePart)attachment;

                        email.Attachments.Add(part.FileName);
                    }
                }
            }
            // ]

            // von Herr Rill
            // [
            var currentAccount = account;
            var currentEmail = email;

            dispatcher.BeginInvoke((Action)(() =>
            {
                currentAccount.Emails.Add(currentEmail);
            }));
            // ]
        }

        /// <summary>
        /// Löscht eine Email des übergebenen Accounts auf dem Server. (Funktioniert noch nicht)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="account"></param>
        public async void deleteMessageAsync(EmailViewModel email, AccountViewModel account)
        {
            if (account != null)
            {
                if (account.UseImap)
                {
                    try
                    {
                        using (ImapClient imapClient = new ImapClient())
                        {
                            await imapClient.ConnectAsync(account.ImapPop3Server, account.ImapPop3Port, true);

                            imapClient.Authenticate(account.User, account.Password);

                            imapClient.Inbox.Open(FolderAccess.ReadWrite);

                            // zum Löschen markieren
                            // TODO funktioniert noch nicht
                            // Es wird zwar der Flag gesetzt zum Löschen aber die E-Mail bleibt trotzdem auf dem Server vorhanden.
                            imapClient.Inbox.AddFlags(email.Id, MessageFlags.Deleted, false);

                            Console.WriteLine("Sollte Email auf Server löschen! Funktioniert aber nicht.");

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
    }
}
