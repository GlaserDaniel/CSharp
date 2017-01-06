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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private SettingsController settingsController;
        private Account selectedAccountToRemove;

        public AccountWindow()
        {
            InitializeComponent();
            this.selectedAccountToRemove = null;
            Console.WriteLine("Constr");
            Show();
        }
        
        public AccountWindow(SettingsController settingsController) : this()
        {
            Console.WriteLine("Account hinzufügen (Constr mit SettingsController)");
            this.settingsController = settingsController;
            Title = "Account hinzufügen";

            // TODO nur zu Testzwecken
            userTextBox.Text = "danielglaser@gmx.de";
            emailTextBox.Text = "danielglaser@gmx.de";
            passwordBox.Password = "VGNFZ11s";
            useImapCheckBox.IsChecked = true;
            imapPop3ServerTextBox.Text = "pop.gmx.net";
            smtpServerTextBox.Text = "995";
            imapPop3PortTextBox.Text = "mail.gmx.net";
            smtpPortTextBox.Text = "587";
        }

        public AccountWindow(SettingsController settingsController, Account selectedAccountToEdit) : this()
        {
            Console.WriteLine("Account bearbeiten (Constr mit Account)");
            this.settingsController = settingsController;
            selectedAccountToRemove = selectedAccountToEdit;
            Title = "Account bearbeiten";

            DataContext = this.selectedAccountToRemove;
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

                Close();
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
