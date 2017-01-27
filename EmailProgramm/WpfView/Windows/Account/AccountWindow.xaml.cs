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
using System.Net.Sockets;

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
            // wieder die Standard Farbe nehmen falls man beim zweiten Mal was anderes falsch hat, 
            // damit das erste nicht mehr rot ist.
            shownameTextBox.ClearValue(TextBox.BorderBrushProperty);
            shownameTextBox.ClearValue(TextBox.BorderBrushProperty);
            userTextBox.ClearValue(TextBox.BorderBrushProperty);
            emailTextBox.ClearValue(TextBox.BorderBrushProperty);
            passwordBox.ClearValue(TextBox.BorderBrushProperty);
            imapPop3ServerTextBox.ClearValue(TextBox.BorderBrushProperty);
            imapPop3PortTextBox.ClearValue(TextBox.BorderBrushProperty);
            smtpServerTextBox.ClearValue(TextBox.BorderBrushProperty);
            smtpPortTextBox.ClearValue(TextBox.BorderBrushProperty);

            if (settingsViewModel != null)
            {
                //Account hinzufügen
                try
                {
                    BindingGroup.CommitEdit();
                    if (!BindingGroup.HasValidationError)
                    {
                        //settingsViewModel.addAccount((AccountViewModel)DataContext);
                        settingsViewModel.addAccount(shownameTextBox.Text, userTextBox.Text, emailTextBox.Text,
                            passwordBox.Password, (bool)imapRadioButton.IsChecked, imapPop3ServerTextBox.Text,
                            imapPop3PortTextBox.Text, smtpServerTextBox.Text, smtpPortTextBox.Text);

                        DialogResult = true;
                    }
                }
                catch (ShownameEmptyException)
                {
                    shownameTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der Anzeigename ist leer!", "Anzeigename fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (UserEmptyException)
                {
                    userTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der User ist leer!", "User fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (EmailEmptyException)
                {
                    emailTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Die Email ist leer!", "Email fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (PasswordEmptyException)
                {
                    passwordBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Das Passwort ist leer!", "Passwort fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3ServerEmptyException)
                {
                    imapPop3ServerTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der IMAP/POP3-Server ist leer!", "IMAP/POP3-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    imapPop3PortTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der IMAP/POP3-Port ist leer!", "IMAP/POP3-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortFormatException)
                {
                    imapPop3PortTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der IMAP/POP3-Port ist keine Zahl!", "IMAP/POP3-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPServerEmptyException)
                {
                    smtpServerTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der SMTP-Server ist leer!", "SMTP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPPortEmtpyException)
                {
                    smtpPortTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der SMTP-Port ist leer!", "SMTP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (SMTPPortFormatException)
                {
                    smtpPortTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Der SMTP-Port ist keine Zahl!", "SMTP-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                BindingGroup.CommitEdit();
                if (!BindingGroup.HasValidationError)
                {
                    DialogResult = true;
                }
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            BindingGroup.CancelEdit();
            DialogResult = false;
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
                    Task<bool> resultTask = settingsViewModel.TestIMAPServerAsync(imapPop3ServerTextBox.Text, imapPop3PortTextBox.Text);

                    Task.Run(() =>
                    {
                        resultTask.Wait();
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
                    MessageBox.Show("Der IMAP-Server ist leer!", "IMAP-Server fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    MessageBox.Show("Der IMAP-Port ist leer!", "IMAP-Port fehlt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (IMAPPOP3PortFormatException)
                {
                    MessageBox.Show("Der IMAP-Port ist keine Zahl!", "IMAP-Port falsch", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Der Server antwortet nicht in einer gewissen Zeit! Vielleicht keine Internetverbindung.", "Server antwortet nicht", MessageBoxButton.OK, MessageBoxImage.Warning);
                } catch (UriFormatException)
                {
                    MessageBox.Show("Die Server Adresse ist keine gültige Internet-Adresse. Bitte prüfen Sie sie noch einmal", "Server Adresse ungültig", MessageBoxButton.OK, MessageBoxImage.Warning);
                } catch (SocketException)
                {
                    MessageBox.Show("Der Server antwortet nicht in einer gewissen Zeit! Vielleicht keine Internetverbindung.", "Server antwortet nicht", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex);
                    MessageBox.Show("Es ist ein unerwarterter Fehler aufgetreten. Fehler für Entwickler: " + ex, "Unerwarteter Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                catch (TimeoutException)
                {
                    MessageBox.Show("Der Server antwortet nicht in einer gewissen Zeit! Vielleicht keine Internetverbindung.", "Server antwortet nicht", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            catch (TimeoutException)
            {
                MessageBox.Show("Der Server antwortet nicht in einer gewissen Zeit! Vielleicht keine Internetverbindung.", "Server antwortet nicht", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            // Bei ShowDialog nicht mehr notwändig
            //if (e.Key == Key.Enter)
            //{
            //    SaveAccount_Click(sender, e);
            //}
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
