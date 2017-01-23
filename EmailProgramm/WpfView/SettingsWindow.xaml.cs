using System;
using System.Windows;
using System.Windows.Data;
using Common.ViewModel;

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
            BindingGroup.BeginEdit();
            //Console.WriteLine("selectedAccount: " + ((SettingsViewModel)DataContext).selectedAccount.email);
        }

        public SettingsWindow(AccountListViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            Show();
            BindingGroup.BeginEdit();
        }

        private void LoadData()
        {
            DataContext = AccountListViewModel.Instance;
            //AccountsComboBox.Items.Clear();
            //foreach (Account account in ((SettingsViewModel)DataContext).Accounts)
            //{
            //    AccountsComboBox.Items.Add(account);
            //}
            if (((AccountListViewModel)DataContext).SelectedAccountIndex >= 0)
            {
                AccountsComboBox.SelectedItem = ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex];
            }
        }

        public void Refresh()
        {
            LoadData();
        }

        // TODO entfernen falls unnötig
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            Console.WriteLine("OptionWindow gotFocus");
            LoadData();
        }

        void window_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("OptionWindow activated");
            if (((AccountListViewModel)DataContext).SelectedAccountIndex == -1 && ((AccountListViewModel)DataContext).Accounts.Count >= 1)
            {
                AccountsComboBox.SelectedItem = ((AccountListViewModel)DataContext).Accounts[0];
            }
        }

        private void AppendSettings_Click(object sender, RoutedEventArgs e)
        {
            //((SettingsViewModel)DataContext).selectedAccount = (Account)AccountsComboBox.SelectedItem;
            ((AccountListViewModel)DataContext).SelectedAccountIndex = AccountsComboBox.SelectedIndex;
            BindingGroup.CommitEdit();
            ((AccountListViewModel)DataContext).saveAsync();
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            BindingGroup.CancelEdit();
            Close();
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((AccountListViewModel)DataContext);
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow((AccountListViewModel)DataContext, (AccountViewModel)AccountsComboBox.SelectedItem);
        }

        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountViewModel accountToRemove = (AccountViewModel)AccountsComboBox.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Willst du den Account " + accountToRemove.Email + " wirklich löschen?", "Account Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Ausgewählter Account: " + accountToRemove.ToString());

                ((AccountListViewModel)DataContext).removeAccount(accountToRemove);

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
