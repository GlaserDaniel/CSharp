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
            SettingsViewModel settingsController = new SettingsViewModel();
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
            if (DataContext != null)
            {
                Account account = (Account)DataContext;
                
                var progressHandler = new Progress<double>(value =>
                {
                    ProgressBar.Value = value;
                });

                Task t = new EmailViewModel().receiveEmails(account, progressHandler, Dispatcher);

                //Console.WriteLine("Task Status: " + t.Status.ToString());
                //loadData();
            } else
            {
                MessageBox.Show("Kein Account angelegt oder ausgewählt.", "Kein Account", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EmailsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ShowEmailWindow((Email)((FrameworkElement)e.OriginalSource).DataContext);
        }

        // TODO onClose Application.Current.Shutdown();
    }
}
