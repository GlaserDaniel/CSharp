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
            SettingsController settingsController = new SettingsController();
            if (settingsController.selectedAccountIndex >= 0)
            {
                DataContext = settingsController.Accounts[settingsController.selectedAccountIndex];
            }
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
            new EmailController().receiveEmails();
        }

        private void EmailsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ShowEmailWindow((Email)((FrameworkElement)e.OriginalSource).DataContext);
        }

        // TODO onClose Application.Current.Shutdown();
    }
}
