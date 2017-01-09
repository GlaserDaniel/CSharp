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
using System.Text.RegularExpressions;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private SettingsViewModel settingsController;
        private Account selectedAccountToRemove;
        //private SettingsWindow settingsWindow;

        public AccountWindow()
        {
            InitializeComponent();
            this.selectedAccountToRemove = null;
            Console.WriteLine("Constr");
            Show();
        }
        
        public AccountWindow(SettingsViewModel settingsController) : this()
        {
            Console.WriteLine("Account hinzufügen (Constr mit SettingsController)");
            this.settingsController = settingsController;
            //this.settingsWindow = settingsWindow;
            Title = "Account hinzufügen";

            // TODO nur zu Testzwecken
            Account testAccount = new Account("danielglaser@gmx.de", "danielglaser@gmx.de", "VGNFZ11s", false, "pop.gmx.net", 995, "mail.gmx.net", 587);
            DataContext = testAccount;
            passwordBox.Password = "VGNFZ11s";
        }

        public AccountWindow(SettingsViewModel settingsController, Account selectedAccountToEdit) : this()
        {
            Console.WriteLine("Account bearbeiten (Constr mit Account)");
            this.settingsController = settingsController;
            selectedAccountToRemove = selectedAccountToEdit;
            Title = "Account bearbeiten";

            DataContext = selectedAccountToRemove;
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            bool userResult = true;
            bool emailResult = true;
            bool passwordResult = true;
            bool imapPop3ServerResult = true;
            bool imapPop3PortResult = true;
            bool smtpServerResult = true;
            bool smtpPortResult = true;

            string user = userTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            bool useImap = (bool)useImapCheckBox.IsChecked;
            string imapPop3Server = imapPop3ServerTextBox.Text;
            string smtpServer = smtpServerTextBox.Text;

            int imapPop3Port = 0;
            if (String.IsNullOrEmpty(imapPop3PortTextBox.Text))
            {
                imapPop3PortResult = false;
            } else {
                imapPop3Port = int.Parse(imapPop3PortTextBox.Text);
            }

            int smtpPort = 0;
            if (String.IsNullOrEmpty(smtpPortTextBox.Text))
            {
                smtpPortResult = false;
            }
            else
            {
                smtpPort = int.Parse(smtpPortTextBox.Text);
            }


            if (String.IsNullOrEmpty(user))
            {
                MessageBox.Show("Der User ist leer!", "User fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                userResult = false;
            }
            if (String.IsNullOrEmpty(email) && userResult)
            {
                MessageBox.Show("Die Email ist leer!", "Email fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                emailResult = false;
            }
            if (String.IsNullOrEmpty(password) && userResult && emailResult)
            {
                MessageBox.Show("Das Passwort ist leer!", "Passwort fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                passwordResult = false;
            }
            if (String.IsNullOrEmpty(imapPop3Server) && userResult && emailResult && passwordResult)
            {
                MessageBox.Show("Der IMAP/POP3-Server ist leer!", "IMAP/POP3-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                imapPop3ServerResult = false;
            }
            if (String.IsNullOrEmpty(smtpServer) && userResult && emailResult && passwordResult && imapPop3ServerResult)
            {
                MessageBox.Show("Der SMTP-Server ist leer!", "SMTP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                smtpServerResult = false;
            }

            if (!imapPop3PortResult && userResult && emailResult && passwordResult && imapPop3ServerResult && smtpServerResult)
            {
                MessageBox.Show("Der IMAP/POP3-Port  ist leer!", "IMAP/POP3-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (!smtpPortResult && userResult && emailResult && passwordResult && imapPop3ServerResult && smtpServerResult && imapPop3PortResult)
            {
                MessageBox.Show("Der SMTP-Port ist leer!", "SMTP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


            if (userResult && emailResult && passwordResult && imapPop3ServerResult && imapPop3PortResult && smtpServerResult && smtpPortResult)
            {
                // falls ein Account gesetzt wurde diesen zuerst löschen
                if (selectedAccountToRemove != null)
                {
                    //alten Account löschen
                    settingsController.removeAccount(selectedAccountToRemove);
                }
                
                //Account hinzufügen
                settingsController.addAccount(user, email, password, useImap, imapPop3Server, imapPop3Port, smtpServer, smtpPort);

                //settingsWindow.Refresh();

                Close();
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void imapPop3PortTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void smtpPortTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void testImapPop3ServerButton_Click(object sender, RoutedEventArgs e)
        {
            // EmailController.testServer();
            if ((bool)useImapCheckBox.IsChecked)
            {
                //new EmailController().TestImapServer(imapPop3ServerTextBox.Text, int imapPop3PortTextBox.Text)
            }
        }

        private void testSmtpServerButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
