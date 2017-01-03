using System;
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

        public EmailController()
        {
            // TODO Account auslesen
            this.account = new Account("danielglaser9@gmail.com", "vgnFZ11s$", "smtp.googlemail.com", 465);
        }

        // Snippet von www.code-bude.net
        // https://code-bude.net/2011/06/14/emails-versenden-in-csharp/
        public void sendEmail(Email email)
        {
            MailMessage mailMessage = new MailMessage();

            //Absender konfigurieren
            mailMessage.From = new MailAddress(email.sender);
            
            //Empfänger konfigurieren
            mailMessage.To.Add(email.receiver);

            //Betreff einrichten
            mailMessage.Subject = email.subject;

            //Hinzufügen der eigentlichen Nachricht
            mailMessage.Body = email.message;

            //Ausgangsserver initialisieren
            SmtpClient MailClient = new SmtpClient(account.server, account.port);

            if (account.user.Length > 0 && account.user != string.Empty)
            {
                //Login konfigurieren
                MailClient.Credentials = new System.Net.NetworkCredential(account.user, account.password);
            }

            //Email absenden
            try {
                MailClient.Send(mailMessage);
            } catch (SmtpException e)
            {
                Console.WriteLine("SMTP Exception: " + e);
            }
        }

        public void receiveEmail()
        {

        }
    }
}
