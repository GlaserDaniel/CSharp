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
        public SettingsWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
            //Console.WriteLine("selectedAccount: " + ((SettingsController)DataContext).selectedAccount.email);
        }

        private void LoadData()
        {
            DataContext = new SettingsViewModel();
            //AccountsComboBox.Items.Clear();
            //foreach (Account account in ((SettingsController)DataContext).Accounts)
            //{
            //    AccountsComboBox.Items.Add(account);
            //}
            if (((SettingsViewModel)DataContext).selectedAccountIndex >= 0)
            {
                AccountsComboBox.SelectedItem = ((SettingsViewModel)DataContext).Accounts[((SettingsViewModel)DataContext).selectedAccountIndex];
            }
        }

        public void Refresh()
        {
            LoadData();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Console.WriteLine("OptionWindow gotFocus");
            LoadData();
        }

        void window_Activated(object sender, EventArgs e)
        {
            //Console.WriteLine("OptionWindow activated");
            //LoadData();
        }

        private void AppendSettings_Click(object sender, RoutedEventArgs e)
        {
            //((SettingsViewModel)DataContext).selectedAccount = (Account)AccountsComboBox.SelectedItem;
            ((SettingsViewModel)DataContext).selectedAccountIndex = AccountsComboBox.SelectedIndex;
            ((SettingsViewModel)DataContext).appendSettings();
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((SettingsViewModel)DataContext);
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((SettingsViewModel)DataContext, (Account)AccountsComboBox.SelectedItem);
        }

        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            Account accountToRemove = (Account)AccountsComboBox.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Willst du den Account " + accountToRemove.email + " wirklich löschen?", "Account Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Ausgewählter Account: " + accountToRemove.ToString());

                ((SettingsViewModel)DataContext).removeAccount(accountToRemove);

                //LoadData();

                //AccountsComboBox.Items.Remove(AccountsComboBox.SelectedItem);

                if (AccountsComboBox.SelectedItem == null)
                {
                    if (AccountsComboBox.Items.Count > 0)
                    {
                        AccountsComboBox.SelectedItem = AccountsComboBox.Items.GetItemAt(0);
                    }
                }
            }
        }
    }
}
