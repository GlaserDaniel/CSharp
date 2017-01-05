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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        SettingsController settingsController { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
            this.settingsController = new SettingsController();
            LoadData();
            Show();
        }

        private void LoadData()
        {
            foreach (Account account in settingsController.accounts) {
                AccountsComboBox.Items.Add(account);
            }

            AccountsComboBox.SelectedItem = settingsController.accounts[0];

            DataContext = settingsController;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Console.WriteLine("OptionWindow gotFocus");
            LoadData();
        }

        private void AppendSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO Einstellungen übernehmen
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow(settingsController);
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow(settingsController, (Account)AccountsComboBox.SelectedItem);
        }

        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Willst du den Account " + ((Account)AccountsComboBox.SelectedItem).email + " wirklich löschen?", "Account Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                settingsController.removeAccount((Account)AccountsComboBox.SelectedItem);
                AccountsComboBox.Items.Remove(AccountsComboBox.SelectedItem);
                if (AccountsComboBox.Items.Count > 0)
                {
                    AccountsComboBox.SelectedItem = AccountsComboBox.Items.GetItemAt(0);
                }
            }
        }
    }
}
