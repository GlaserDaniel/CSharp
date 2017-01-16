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
            receiverTextBox.Text = email.Sender;
            subjectTextBox.Text = email.Subject;
            messageTextBox.Text = email.Message;
        }

        private void LoadData()
        {
            settingsViewModel = new AccountListViewModel();

            //foreach (Account account in settingsViewModel.Accounts)
            //{
            //    senderComboBox.Items.Add(((Account) account).email);
            //}

            DataContext = settingsViewModel;

            if (((AccountListViewModel)DataContext).selectedAccountIndex != -1)
            {
                senderComboBox.SelectedIndex = ((AccountListViewModel)DataContext).selectedAccountIndex;
            }
        }

        private void SendEmail_Click(object senderObject, RoutedEventArgs e)
        {
            //string sender = senderComboBox.Text;
            AccountViewModel senderAccount = (AccountViewModel)senderComboBox.SelectedItem;
            string receiver = receiverTextBox.Text;
            string subject = subjectTextBox.Text;
            string message = messageTextBox.Text;

            bool receiverResult = true;
            MessageBoxResult subjectResult = MessageBoxResult.Yes;
            MessageBoxResult messageResult = MessageBoxResult.Yes;

            if (String.IsNullOrEmpty(receiver))
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

                emailService.sendEmail(senderAccount, receiver, subject, message);

                Close();
            }
        }

        private void AbortSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
