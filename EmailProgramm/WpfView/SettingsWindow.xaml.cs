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
            DataContext = new SettingsController();
            AccountsComboBox.Items.Clear();
            foreach (Account account in ((SettingsController)DataContext).accounts)
            {
                AccountsComboBox.Items.Add(account);
            }
            if (((SettingsController)DataContext).selectedAccountIndex >= 0)
            {
                AccountsComboBox.SelectedItem = ((SettingsController)DataContext).accounts[((SettingsController)DataContext).selectedAccountIndex];
            }
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
            ((SettingsController)DataContext).selectedAccount = (Account)AccountsComboBox.SelectedItem;
            ((SettingsController)DataContext).appendSettings();
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((SettingsController)DataContext);
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((SettingsController)DataContext, (Account)AccountsComboBox.SelectedItem);
        }

        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            Account accountToRemove = (Account)AccountsComboBox.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Willst du den Account " + accountToRemove.email + " wirklich löschen?", "Account Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Ausgewählter Account: " + accountToRemove.ToString());

                ((SettingsController)DataContext).removeAccount(accountToRemove);

                AccountsComboBox.Items.Remove(AccountsComboBox.SelectedItem);

                if (AccountsComboBox.Items.Count > 0)
                {
                    AccountsComboBox.SelectedItem = AccountsComboBox.Items.GetItemAt(0);
                }
            }
        }
    }
}
