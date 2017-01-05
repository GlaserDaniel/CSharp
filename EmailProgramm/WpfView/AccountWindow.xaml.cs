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
        private Account selectedAccount;

        public AccountWindow()
        {
            InitializeComponent();
            this.selectedAccount = null;
            Console.WriteLine("Constr");
            Show();
        }
        
        public AccountWindow(SettingsController settingsController) : this()
        {
            Console.WriteLine("Constr mit Con");
            this.settingsController = settingsController;
            Title = "Account hinzufügen";

            Show();
        }

        public AccountWindow(SettingsController settingsController, Account selectedAccount) : this()
        {
            Console.WriteLine("Constr mit Account");
            this.settingsController = settingsController;
            this.selectedAccount = selectedAccount;
            Title = "Account bearbeiten";

            DataContext = this.selectedAccount;

            Show();
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccount != null)
            {
                settingsController.removeAccount(selectedAccount);
            }
            settingsController.addAccount(userTextBox.Text, emailTextBox.Text, passwordBox.Password, serverTextBox.Text, int.Parse(portTextBox.Text));
            
            Close();
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
