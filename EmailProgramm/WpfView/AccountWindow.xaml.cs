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
using System.Text.RegularExpressions;
using Common.ViewModel;
using Common.Services;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private AccountListViewModel settingsViewModel;
        //private SettingsWindow settingsWindow;

        public AccountWindow()
        {
            InitializeComponent();
            Console.WriteLine("Constr");
            Show();
        }
        
        public AccountWindow(AccountListViewModel settingsViewModel) : this()
        {
            Console.WriteLine("Account hinzufügen (Constr mit SettingsViewModel)");
            this.settingsViewModel = settingsViewModel;
            //this.settingsWindow = settingsWindow;
            Title = "Account hinzufügen";
        }

        public AccountWindow(AccountListViewModel settingsViewModel, AccountViewModel selectedAccountToEdit) : this()
        {
            Console.WriteLine("Account bearbeiten (Constr mit Account)");
            this.settingsViewModel = settingsViewModel;
            Title = "Account bearbeiten";

            // Account als DataContext setzen
            DataContext = selectedAccountToEdit;
            // Passwort aus dem Account holen
            passwordBox.Password = selectedAccountToEdit.Password;

            BindingGroup.BeginEdit();
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

            //string showname = 
            string user = userTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            bool useImap = (bool)imapRadioButton.IsChecked;
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
                if (DataContext == null)
                {
                    //Account hinzufügen
                    settingsViewModel.addAccount("", user, email, password, useImap, imapPop3Server, imapPop3Port, smtpServer, smtpPort);
                }

                BindingGroup.CommitEdit();
                Close();
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            BindingGroup.CancelEdit();
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
            testImapPop3ServerButton.Background = Brushes.White;

            if (String.IsNullOrEmpty(imapPop3ServerTextBox.Text))
            {
                MessageBox.Show("Der IMAP/POP3-Server ist leer!", "IMAP/POP3-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrEmpty(imapPop3PortTextBox.Text))
            {
                MessageBox.Show("Der IMAP/POP3-Port ist leer!", "IMAP/POP3-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if ((bool)imapRadioButton.IsChecked)
            {
                Task<bool> resultTask = new EmailService().TestImapServer(imapPop3ServerTextBox.Text, int.Parse(imapPop3PortTextBox.Text));

                Task.Run(() =>
                {
                    if (resultTask.Result)
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            testImapPop3ServerButton.Background = Brushes.Green;
                        }));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            testImapPop3ServerButton.Background = Brushes.Red;
                        }));
                    }
                });
            } else
            {
                Task<bool> resultTask = new EmailService().TestPop3Server(imapPop3ServerTextBox.Text, int.Parse(imapPop3PortTextBox.Text));

                Task.Run(() =>
                {
                    if (resultTask.Result)
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            testImapPop3ServerButton.Background = Brushes.Green;
                        }));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            testImapPop3ServerButton.Background = Brushes.Red;
                        }));
                    }
                });
            }
        }

        private void testSmtpServerButton_Click(object sender, RoutedEventArgs e)
        {
            testSmtpServerButton.Background = Brushes.White;

            if (String.IsNullOrEmpty(smtpServerTextBox.Text))
            {
                MessageBox.Show("Der SMTP-Server ist leer!", "SMTP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrEmpty(smtpPortTextBox.Text))
            {
                MessageBox.Show("Der SMTP-Port ist leer!", "SMTP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Task<bool> resultTask = new EmailService().TestSmtpServer(smtpServerTextBox.Text, int.Parse(smtpPortTextBox.Text));

            Task.Run(() =>
            {
                if (resultTask.Result)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        testSmtpServerButton.Background = Brushes.Green;
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        testSmtpServerButton.Background = Brushes.Red;
                    }));
                }
            });
        }
    }
}
