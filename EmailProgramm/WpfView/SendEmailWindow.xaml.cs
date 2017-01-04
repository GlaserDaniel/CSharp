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
using Common;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for SendEmailWindow.xaml
    /// </summary>
    public partial class SendEmailWindow : Window
    {
        SettingsController settingsController { get; set; }

        public SendEmailWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
        }

        private void LoadData()
        {
            settingsController = new SettingsController();

            DataContext = settingsController;
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            // TODO Email senden
            Email email = new Email();

            email.sender = senderTextBox.Text;
            email.receiver = receiverTextBox.Text;
            email.subject = subjectTextBox.Text;
            email.message = messageTextBox.Text;

            EmailController emailController = new EmailController();
            
            emailController.sendEmail(email);
            Close();
        }

        private void AbortSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
