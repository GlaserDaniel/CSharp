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
        /// <summary>
        /// Standard Konstruktor. Öffnet das SettingsWindow und läd die Daten aus der Instance.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
            BindingGroup.BeginEdit();
        }

        private void LoadData()
        {
            DataContext = AccountListViewModel.Instance;
        }

        private void CloseSettings_Click(object sender, RoutedEventArgs e)
        {
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
            new AccountWindow((AccountViewModel)AccountsListView.SelectedItem);
        }

        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountViewModel accountToRemove = (AccountViewModel)AccountsListView.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Willst du den Account " + accountToRemove.Showname + " wirklich löschen?", "Account Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Ausgewählter Account zum löschen: " + accountToRemove.ToString());

                ((AccountListViewModel)DataContext).removeAccount(accountToRemove);
            }
        }

        private void AccountsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new AccountWindow((AccountViewModel)AccountsListView.SelectedItem);
        }

        private void StandardAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsListView.SelectedIndex != -1)
            {
                AccountListViewModel.Instance.SelectedAccountIndex = AccountsListView.SelectedIndex;
            }
            else
            {
                MessageBox.Show("Kein Account ausgewählt.", "Kein Account ausgewählt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
