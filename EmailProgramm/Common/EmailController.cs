﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

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
        public void sendEmail(Email email)
        {
            MailMessage mailMessage = new MailMessage();

            //Absender konfigurieren
            mailMessage.From = new MailAddress(email.sender);
            
            //Empfänger konfigurieren
            mailMessage.To.Add(new MailAddress(email.receiver.Trim()));

            //Betreff einrichten
            mailMessage.Subject = email.subject;

            //Hinzufügen der eigentlichen Nachricht
            mailMessage.Body = email.message;

            //Ausgangsserver initialisieren
            SmtpClient MailClient = new SmtpClient(account.server, account.port);

            // SSL aktivieren
            // Scheint unnötig da es sowieso gesetzt wird (funktioniert auch ohne)
            MailClient.EnableSsl = true;

            if (account.user.Length > 0 && account.user != string.Empty)
            {
                //Login konfigurieren
                MailClient.Credentials = new System.Net.NetworkCredential(account.user, account.password);
                Console.WriteLine("Password: " + account.password);
            }

            Console.WriteLine(mailMessage.To);

            //Email absenden
            try {
                MailClient.SendMailAsync(mailMessage); // TODO SendMailAsync
            } catch (SmtpException e)
            {
                // TODO Benachrichtigung das es nicht funktioniert hat!
                Console.WriteLine("SMTP Exception: " + e);
            }
        }

        public void receiveEmail()
        {

        }
    }
}
