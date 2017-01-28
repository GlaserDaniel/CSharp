using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Common.ViewModel;
using Common.Services;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SendEmailWindow.xaml
    /// </summary>
    public partial class SendEmailWindow : Window
    {
        AccountListViewModel settingsViewModel { get; set; }
        List<string> attachments;

        public SendEmailWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
        }

        /// <summary>
        /// Konstruktor der eine Email mit annimmt um die Daten dieser im Fenster zu setzen.
        /// Wird für Antworten und Weiterleiten genutzt.
        /// </summary>
        /// <param name="email"></param>
        public SendEmailWindow(EmailViewModel email) : this()
        {
            if (!String.IsNullOrEmpty(email.Sender))
            {
                // Anwortmail
                receiverTextBox.Text = email.Sender;
            }
            subjectTextBox.Text = email.Subject;
            messageTextBox.Text = email.Message;

            if (String.IsNullOrEmpty(email.Sender))
            {
                receiverTextBox.Focus();
            }
            else
            {
                messageTextBox.Focus();
            }
        }

        public SendEmailWindow(AccountListViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            Show();
        }

        public SendEmailWindow(AccountListViewModel dataContext, EmailViewModel email)
        {
            InitializeComponent();
            DataContext = dataContext;
            receiverTextBox.Text = email.Sender;
            subjectTextBox.Text = email.Subject;
            messageTextBox.Text = email.Message;
            messageTextBox.Focus();
            Show();
        }

        private void LoadData()
        {
            settingsViewModel = AccountListViewModel.Instance;

            DataContext = settingsViewModel;

            if (((AccountListViewModel)DataContext).SelectedAccountIndex != -1)
            {
                senderComboBox.SelectedIndex = ((AccountListViewModel)DataContext).SelectedAccountIndex;
            }
        }

        private void SendEmail_Click(object senderObject, RoutedEventArgs e)
        {
            //string sender = senderComboBox.Text;
            AccountViewModel senderAccount = (AccountViewModel)senderComboBox.SelectedItem;
            string receiverString = receiverTextBox.Text;
            string subject = subjectTextBox.Text;
            string message = messageTextBox.Text;

            bool receiverResult = true;
            MessageBoxResult subjectResult = MessageBoxResult.Yes;
            MessageBoxResult messageResult = MessageBoxResult.Yes;

            if (String.IsNullOrEmpty(receiverString))
            {
                MessageBox.Show("Der Empfänger ist leer!", "Empfänger fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                receiverResult = false;
            }
            if (String.IsNullOrEmpty(subject) && receiverResult)
            {
                subjectResult = MessageBox.Show("Der Betreff ist leer! Trotzdem senden?", "Betreff fehlt", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (String.IsNullOrEmpty(message) && receiverResult && subjectResult == MessageBoxResult.Yes)
            {
                messageResult = MessageBox.Show("Die Nachricht ist leer! Trotzdem senden?", "Betreff fehlt", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }

            if (receiverResult && subjectResult == MessageBoxResult.Yes && messageResult == MessageBoxResult.Yes)
            {
                EmailService emailService = new EmailService();

                // Empfänger zu List machen
                List<string> receivers = new List<string>();

                receiverString.Trim();

                if (receiverString.Contains(','))
                {
                    receivers = receiverString.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                {
                    receivers.Add(receiverString);
                }

                Console.WriteLine("receivers: " + receivers.ToString());

                // CCs zu List machen
                List<string> ccReceivers = new List<string>();

                string ccReceiversString = CCTextBox.Text;

                if (!String.IsNullOrEmpty(ccReceiversString))
                {

                    ccReceiversString.Trim();

                    if (ccReceiversString.Contains(','))
                    {
                        ccReceivers = ccReceiversString.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                    {
                        ccReceivers.Add(ccReceiversString);
                    }
                }

                // BCCs zu List machen
                List<string> bccReceivers = new List<string>();

                string bccReceiversString = BCCTextBox.Text;
                if (!String.IsNullOrEmpty(bccReceiversString))
                {
                    bccReceiversString.Trim();

                    if (bccReceiversString.Contains(','))
                    {
                        bccReceivers = bccReceiversString.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                    {
                        bccReceivers.Add(bccReceiversString);
                    }
                }

                // Signature ans Ende hinzufügen
                message += senderAccount.Signature;

                try
                {
                    emailService.sendEmailAsync(senderAccount, receivers, ccReceivers, bccReceivers, subject, message, attachments);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Eine E-Mail Adresse ist nicht im richtigen Format.", "Nicht Email-Adressen Format", MessageBoxButton.OK, MessageBoxImage.Error);
                } // TODO genauere Fehlermeldung mit selbst gemachten Exceptions

                Close();
            }
        }

        private void AbortSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chooseFilesButton_Click(object sender, RoutedEventArgs e)
        {
            // von https://msdn.microsoft.com/en-us/library/aa969773.aspx#Common_Dialogs
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ""; // Default file extension
            dlg.Filter = "All Files|*.*"; // Filter files by extension
            dlg.Multiselect = true;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                attachments = dlg.FileNames.ToList();

                string attachmentsString = "Keine Datei";
                if (attachments.Count > 0)
                {
                    attachmentsString = attachments[0].Substring(attachments[0].LastIndexOf("\\") + 1);
                    for (int i = 1; i < attachments.Count; i++)
                    {
                        attachmentsString += ", " + attachments[i].Substring(attachments[i].LastIndexOf("\\") + 1);
                    }
                }

                choosenFilesLabel.Content = "Ausgewählte Dateien: " + attachmentsString;
            }
        }
    }
}
