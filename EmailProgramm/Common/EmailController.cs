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

namespace Common
{
    public class EmailController
    {
        Account account;

        public EmailController(Account account)
        {
            this.account = account;
        }

        // Snippet von www.code-bude.net
        // https://code-bude.net/2011/06/14/emails-versenden-in-csharp/
        public void sendEmail(string sender, string receiver, string subject, string message)
        {
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
            try {
                smtpClient.SendMailAsync(mailMessage); // TODO SendMailAsync
            } catch (SmtpException e)
            {
                // TODO Benachrichtigung das es nicht funktioniert hat!
                Console.WriteLine("SMTP Exception: " + e);
            }
        }

        public void receiveEmails()
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
                } catch (SocketException se)
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

                        int count = client.Count;
                        if (client.Count > 10)
                        {
                            count = 10;
                        }

                        for (int i = 0; i < count; i++)
                        {
                            var message = client.GetMessage(i);
                            
                            // write the message to a file
                            //message.WriteTo(string.Format("{0}.msg", i));

                            Console.WriteLine("Message " + i + ": " + message.Subject);

                            Email email = new Email();

                            email.sender = message.From.ToString();
                            email.receiver = message.To.ToString();
                            email.subject = message.Subject.ToString();
                            email.message = message.Body.ToString();

                            //Console.WriteLine("Email: " + email.ToString());

                            account.emails.Add(email);

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
            // TODO save
        }
    }
}
