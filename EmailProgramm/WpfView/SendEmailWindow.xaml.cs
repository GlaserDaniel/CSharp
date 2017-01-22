using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        public SendEmailWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
        }

        public SendEmailWindow(EmailViewModel email) : this()
        {
            if (!String.IsNullOrEmpty(email.Sender))
            {
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

            //foreach (Account account in settingsViewModel.Accounts)
            //{
            //    senderComboBox.Items.Add(((Account) account).email);
            //}

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

                List<string> receivers = new List<string>();

                receiverString.Trim();

                if(receiverString.Contains(',')) {
                    receivers = receiverString.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                } else
                {
                    receivers.Add(receiverString);
                }

                Console.WriteLine("receivers: " + receivers.ToString());

                emailService.sendEmail(senderAccount, receivers, subject, message);

                Close();
            }
        }

        private void AbortSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
