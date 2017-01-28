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

            ErrorLabel.Content = "";

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
                            imapPop3PortTextBox.Text, smtpServerTextBox.Text, smtpPortTextBox.Text, signatureTextBox.Text);

                        DialogResult = true;
                    }
                }
                catch (ShownameEmptyException)
                {
                    shownameTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der Anzeigename ist leer!";
                }
                catch (UserEmptyException)
                {
                    userTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der User ist leer!";
                }
                catch (EmailEmptyException)
                {
                    emailTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Die Email ist leer!";
                }
                catch (PasswordEmptyException)
                {
                    passwordBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Das Passwort ist leer!";
                }
                catch (IMAPPOP3ServerEmptyException)
                {
                    imapPop3ServerTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der IMAP/POP3-Server ist leer!";
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    imapPop3PortTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der IMAP/POP3-Port ist leer!";
                }
                catch (IMAPPOP3PortFormatException)
                {
                    imapPop3PortTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der IMAP/POP3-Port ist keine Zahl!";
                }
                catch (SMTPServerEmptyException)
                {
                    smtpServerTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der SMTP-Server ist leer!";
                }
                catch (SMTPPortEmtpyException)
                {
                    smtpPortTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der SMTP-Port ist leer!";
                }
                catch (SMTPPortFormatException)
                {
                    smtpPortTextBox.BorderBrush = Brushes.Red;
                    ErrorLabel.Content = "Der SMTP-Port ist keine Zahl!";
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

        private async void testImapPop3ServerButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            testImapPop3ServerButton.Background = Brushes.White;
            ErrorLabel.Content = "";

            if ((bool)imapRadioButton.IsChecked)
            {
                try
                {
                    bool result = await AccountListViewModel.Instance.TestIMAPServerAsync(imapPop3ServerTextBox.Text, imapPop3PortTextBox.Text);

                    if (result)
                    {
                        testImapPop3ServerButton.Background = Brushes.Green;
                    }
                    else
                    {
                        testImapPop3ServerButton.Background = Brushes.Red;
                    }
                }
                catch (IMAPPOP3ServerEmptyException)
                {
                    ErrorLabel.Content = "Der IMAP-Server ist leer!";
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    ErrorLabel.Content = "Der IMAP-Port ist leer!";
                }
                catch (IMAPPOP3PortFormatException)
                {
                    ErrorLabel.Content = "Der IMAP-Port ist keine Zahl!";
                }
                catch (TimeoutException)
                {
                    ErrorLabel.Content = "Der Server antwortet nicht in einer gewissen Zeit! \nVielleicht keine Internetverbindung.";
                }
                catch (UriFormatException)
                {
                    ErrorLabel.Content = "Die Server Adresse ist keine gültige Internet-Adresse. \nBitte prüfen Sie sie noch einmal";
                }
                catch (SocketException)
                {
                    ErrorLabel.Content = "Der Port ist bei diesem Server entweder nicht verfügbar \noder nicht für Email gedacht.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex);
                    ErrorLabel.Content = "Es ist ein unerwarterter Fehler aufgetreten. \nFehler für Entwickler: \n" + ex;
                }
            }
            else
            {
                try
                {
                    bool result = await AccountListViewModel.Instance.TestPOP3ServerAsync(imapPop3ServerTextBox.Text, imapPop3PortTextBox.Text);

                    if (result)
                    {
                        testImapPop3ServerButton.Background = Brushes.Green;
                    }
                    else
                    {
                        testImapPop3ServerButton.Background = Brushes.Red;
                    }
                }
                catch (IMAPPOP3ServerEmptyException)
                {
                    ErrorLabel.Content = "Der POP3-Server ist leer!";
                }
                catch (IMAPPOP3PortEmptyException)
                {
                    ErrorLabel.Content = "Der POP3-Port ist leer!";
                }
                catch (IMAPPOP3PortFormatException)
                {
                    ErrorLabel.Content = "Der POP3-Port ist keine Zahl!";
                }
                catch (TimeoutException)
                {
                    ErrorLabel.Content = "Der Server antwortet nicht in einer gewissen Zeit! \nVielleicht keine Internetverbindung.";
                }
                catch (UriFormatException)
                {
                    ErrorLabel.Content = "Die Server Adresse ist keine gültige Internet-Adresse. \nBitte prüfen Sie sie noch einmal";
                }
                catch (SocketException)
                {
                    ErrorLabel.Content = "Der Port ist bei diesem Server entweder nicht verfügbar \noder nicht für Email gedacht.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex);
                    ErrorLabel.Content = "Es ist ein unerwarterter Fehler aufgetreten. \nFehler für Entwickler: \n" + ex;
                }
            }
        }

        private async void testSmtpServerButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            testSmtpServerButton.Background = Brushes.White;
            ErrorLabel.Content = "";

            try
            {
                bool result = await AccountListViewModel.Instance.TestSMTPServerAsync(smtpServerTextBox.Text, smtpPortTextBox.Text);

                if (result)
                {
                    testSmtpServerButton.Background = Brushes.Green;
                }
                else
                {
                    testSmtpServerButton.Background = Brushes.Red;
                }
            }
            catch (SMTPServerEmptyException)
            {
                ErrorLabel.Content = "Der SMTP-Server ist leer!";
            }
            catch (SMTPPortEmtpyException)
            {
                ErrorLabel.Content = "Der SMTP-Port ist leer!";
            }
            catch (SMTPPortFormatException)
            {
                ErrorLabel.Content = "Der SMTP-Port ist keine Zahl!";
            }
            catch (TimeoutException)
            {
                ErrorLabel.Content = "Der Server antwortet nicht in einer gewissen Zeit! \nVielleicht keine Internetverbindung.";
            }
            catch (UriFormatException)
            {
                ErrorLabel.Content = "Die Server Adresse ist keine gültige Internet-Adresse. \nBitte prüfen Sie sie noch einmal";
            }
            catch (SocketException)
            {
                ErrorLabel.Content = "Der Port ist bei diesem Server entweder nicht verfügbar \noder nicht für Email gedacht.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                ErrorLabel.Content = "Es ist ein unerwarterter Fehler aufgetreten. \nFehler für Entwickler: \n" + ex;
            }
        }
    }
}
