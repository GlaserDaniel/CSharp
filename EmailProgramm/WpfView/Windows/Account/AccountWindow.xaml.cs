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
using Common.Exceptions;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private AccountListViewModel settingsViewModel;
        //private SettingsWindow settingsWindow;

        public AccountWindow(AccountListViewModel settingsViewModel)
        {
            InitializeComponent();

            Console.WriteLine("Account hinzufügen");
            this.settingsViewModel = settingsViewModel;
            //this.settingsWindow = settingsWindow;
            Title = "Account hinzufügen";

            DataContext = new AccountViewModel();

            BindingGroup.BeginEdit();

            shownameTextBox.Focus();

            ShowDialog();
        }

        public AccountWindow(AccountViewModel selectedAccountToEdit)
        {
            InitializeComponent();

            Console.WriteLine("Account bearbeiten");
            Title = "Account bearbeiten";

            // Account als DataContext setzen
            DataContext = selectedAccountToEdit;
            // Passwort aus dem Account holen
            passwordBox.Password = selectedAccountToEdit.Password;

            BindingGroup.BeginEdit();

            shownameTextBox.Focus();

            ShowDialog();
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (settingsViewModel != null)
            {
                //Account hinzufügen
                try
                {
                    BindingGroup.CommitEdit();
                    settingsViewModel.addAccount(shownameTextBox.Text, userTextBox.Text, emailTextBox.Text,
                        passwordBox.Password, (bool)imapRadioButton.IsChecked, imapPop3ServerTextBox.Text,
                        imapPop3PortTextBox.Text, smtpServerTextBox.Text, smtpPortTextBox.Text);
                    if (!BindingGroup.HasValidationError)
                    {
                        Close();
                    }
                }
                catch (ShownameEmptyException)
                {
                    MessageBox.Show("Der Anzeigename ist leer!", "Anzeigename fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (UserEmptyException)
                {
                    MessageBox.Show("Der User ist leer!", "User fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (EmailEmptyException)
                {
                    MessageBox.Show("Die Email ist leer!", "Email fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (PasswordEmptyException)
                {
                    MessageBox.Show("Das Passwort ist leer!", "Passwort fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3ServerEmptyException)
                {
                    MessageBox.Show("Der IMAP/POP3-Server ist leer!", "IMAP/POP3-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    imapPop3PortTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der IMAP/POP3-Port ist leer!", "IMAP/POP3-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortFormatException)
                {
                    MessageBox.Show("Der IMAP/POP3-Port ist keine Zahl!", "IMAP/POP3-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPServerEmptyException)
                {
                    MessageBox.Show("Der SMTP-Server ist leer!", "SMTP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPPortEmtpyException)
                {
                    MessageBox.Show("Der SMTP-Port ist leer!", "SMTP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPPortFormatException)
                {
                    MessageBox.Show("Der SMTP-Port ist keine Zahl!", "SMTP-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
            else
            {
                BindingGroup.CommitEdit();
                if (!BindingGroup.HasValidationError)
                {
                    Close();
                }
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

            if ((bool)imapRadioButton.IsChecked)
            {
                try
                {
                    Task<bool> resultTask = settingsViewModel.TestIMAPServer(imapPop3ServerTextBox.Text, imapPop3PortTextBox.Text);

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
                } catch (IMAPPOP3ServerEmptyException)
                {
                    MessageBox.Show("Der IMAP-Server ist leer!", "IMAP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                } catch (IMAPPOP3PortEmptyException)
                {
                    MessageBox.Show("Der IMAP-Port ist leer!", "IMAP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                } catch (IMAPPOP3PortFormatException)
                {
                    MessageBox.Show("Der IMAP-Port ist keine Zahl!", "IMAP-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }//TODO mehr Fehlermeldungen
            }
            else
            {
                try
                {
                    Task<bool> resultTask = settingsViewModel.TestPOP3Server(imapPop3ServerTextBox.Text, imapPop3PortTextBox.Text);

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
                catch (IMAPPOP3ServerEmptyException)
                {
                    MessageBox.Show("Der POP3-Server ist leer!", "POP3-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    MessageBox.Show("Der POP3-Port ist leer!", "POP3-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortFormatException)
                {
                    MessageBox.Show("Der POP3-Port ist keine Zahl!", "POP3-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void testSmtpServerButton_Click(object sender, RoutedEventArgs e)
        {
            testSmtpServerButton.Background = Brushes.White;

            try
            {
                Task<bool> resultTask = settingsViewModel.TestSMTPServer(smtpServerTextBox.Text, smtpPortTextBox.Text);

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
            }catch (SMTPServerEmptyException)
            {
                MessageBox.Show("Der SMTP-Server ist leer!", "SMTP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (SMTPPortEmtpyException)
            {
                MessageBox.Show("Der SMTP-Port ist leer!", "SMTP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
            } catch (SMTPPortFormatException)
            {
                MessageBox.Show("Der SMTP-Port ist keine Zahl!", "SMTP-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveAccount_Click(sender, e);
            }
        }

        private int ConvertPort(String port)
        {
            if (String.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException();
            }
            else
            {
                try
                {
                    return int.Parse(port);
                }
                catch (FormatException)
                {
                    throw new FormatException();
                }
            }
        }
    }
}
