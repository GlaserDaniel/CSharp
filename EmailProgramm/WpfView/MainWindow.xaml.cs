using Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            DataContext = new SettingsController().selectedAccount;
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SendEmail_Click");
            new SendEmailWindow();
        }

        private void Optionen_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Optionen_Click");
            new SettingsWindow();
        }

        private void ReceiveEmails_Click(object sender, RoutedEventArgs e)
        {
            SettingsController settingsController = new SettingsController();

            // wenn ein Account als Standard definiert/ausgewählt wurde
            if (settingsController.selectedAccount != null)
            {
                new EmailController(settingsController.selectedAccount).receiveEmails();

                // speichern
                settingsController.appendSettings();

                Console.WriteLine("Emails ausgeben: ");
                foreach (Account account in settingsController.accounts)
                {
                    Console.WriteLine("Account: " + account);
                    foreach (Email email in account.emails)
                    {
                        Console.WriteLine("Email: " + email);
                        new ShowEmailWindow(email);
                    }
                }
            } // TODO else Fehlermeldung
        }

        // TODO onClose Application.Current.Shutdown();
    }
}
