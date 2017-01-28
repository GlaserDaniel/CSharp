using Common.Services;
using Common.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            generateAccountFoldersButton();
        }

        private void loadData()
        {
            AccountListViewModel settingsViewModel = AccountListViewModel.Instance;

            DataContext = settingsViewModel;

            ObservableCollection<EmailViewModel> emails = new ObservableCollection<EmailViewModel>();
            foreach (AccountViewModel account in AccountListViewModel.Instance.Accounts)
            {
                foreach (EmailViewModel email in account.Emails)
                {
                    emails.Add(email);
                }
            }
            EmailsListView.ItemsSource = emails;
            CompleteInboxButton.Background = Brushes.LightBlue;

            // TODO Versuch die Zeilen fett zu markieren in dehnen die Mails ungelesen sind.
            //ObservableCollection<EmailViewModel> emails = ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex].Emails;

            //for (int i = 0; i < emails.Count; i++)
            //{
            //    if (!emails[i].IsRead)
            //    {
            //        ((ListViewItem)EmailsListView.Items[i]).FontWeight = FontWeights.Bold; 
            //    }
            //}
        }

        // TODO anderst lösen! Wegen dem das wenn man den selectedAccount umgestellt hat es nicht die richtige Emails anzeigt.
        void window_Activated(object sender, EventArgs e)
        {
            //Console.WriteLine("MainWindow activated");
            //loadData();
            //generateAccountFoldersButton();
        }

        private void generateAccountFoldersButton()
        {
            foreach (AccountViewModel account in AccountListViewModel.Instance.Accounts)
            {
                bool contains = false;
                foreach (Button childenButton in AccountFoldersStackPanel.Children)
                {
                    if (childenButton.Content.Equals(account.Showname))
                    {
                        contains = true;
                    }
                }
                if (!contains)
                {
                    Button button = new Button();
                    button.Height = 23;
                    button.Padding = new System.Windows.Thickness(5, 0, 5, 0);
                    button.Background = Brushes.White;
                    button.Content = account.Showname;
                    button.Click += (source, e) =>
                    {
                        // alle Buttons wieder weiß färben
                        foreach (Button childenButton in AccountFoldersStackPanel.Children)
                        {
                            childenButton.Background = Brushes.White;
                        }

                        EmailsListView.ItemsSource = account.Emails;
                        button.Background = Brushes.LightBlue;
                    };

                    AccountFoldersStackPanel.Children.Add(button);
                }
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SendEmail_Click");
            if (DataContext == null)
            {
                loadData();
            }
            if (DataContext != null && ((AccountListViewModel)DataContext).SelectedAccountIndex >= 0)
            {
                new SendEmailWindow((AccountListViewModel)DataContext);
            }
            else
            {
                MessageBox.Show("Kein Account angelegt oder ausgewählt.", "Kein Account", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Optionen_Click");
            new SettingsWindow((AccountListViewModel)DataContext);

            generateAccountFoldersButton();
            Console.WriteLine("Nach Settings zurück");
        }

        private void ReceiveEmails_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                loadData();
            }
            if (DataContext != null && ((AccountListViewModel)DataContext).SelectedAccountIndex >= 0)
            {
                AccountViewModel account = ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex];

                var progressHandler = new Progress<double>(value =>
                {
                    ProgressBar.Value = value;
                });

                Task t = new EmailService().receiveEmails(account, progressHandler, Dispatcher);

                Task.Run(() =>
                {
                    t.Wait();
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ((AccountListViewModel)DataContext).saveAsync();
                    }));
                });

                //Console.WriteLine("Task Status: " + t.Status.ToString());
                //loadData();
            }
            else
            {
                MessageBox.Show("Kein Account angelegt oder ausgewählt.", "Kein Account", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EmailsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // weil anscheinend auch AccountViewModel kommen kann
            if (((FrameworkElement)e.OriginalSource).DataContext is EmailViewModel)
            {
                new ShowEmailWindow((EmailViewModel)((FrameworkElement)e.OriginalSource).DataContext);
            }
            else
            {
                Console.WriteLine("selected Item by DoubleClick was no EmailViewModel");
            }
        }

        private void DeleteEmails_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult closingResult = MessageBox.Show("Möchten Sie die Email wirklich löschen?", "Email löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (closingResult == MessageBoxResult.Yes)
            {
                ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex].DeleteSelectedCommandExecute((EmailViewModel)EmailsListView.SelectedItem);
            }

            // funktionieort nicht
            //foreach (var eachItem in EmailsListView.SelectedItems)
            //{
            //    EmailsListView.Items.Remove(eachItem);
            //}

            //List<EmailViewModel> emailsToDelete = new List<EmailViewModel>((IEnumerable<EmailViewModel>)EmailsListView.SelectedItems.GetEnumerator());

            //ObservableCollection<EmailViewModel> emails = ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex].Emails;

            //// TODO Funktioniert noch nicht richtig
            //foreach (var email in emailsToDelete)
            //{
            //    emails.Remove(email);
            //}
        }

        private void EmailsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                MessageBoxResult closingResult = MessageBox.Show("Möchten Sie die Email wirklich löschen?", "Email löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (closingResult == MessageBoxResult.Yes)
                {
                    ((AccountListViewModel)DataContext).Accounts[((AccountListViewModel)DataContext).SelectedAccountIndex].DeleteSelectedCommandExecute((EmailViewModel)EmailsListView.SelectedItem);
                }

                // TODO Funktioniert noch nicht richtig
                //foreach (ListViewItem listViewItem in ((ListView)sender).SelectedItems)
                //{
                //    listViewItem.Remove();
                //}
            }
        }

        /// <summary>
        /// Schließen des Programmes. Hier wird noch einmal nachgefragt ob er das Programm wirklich 
        /// schließen möchte wenn er mehrere Fenster offen hat. 
        /// Falls er das Programm beendet wird gespeichert.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Wenn mehr als ein Fenster (also das noch ein anderes Fenster als das Hauptfenster)
            // offen ist nachfragen ob man das Programm beenden möchte
            int windows = 1;

            // im Debug Modus 2 weil noch ein Diagnose Fenster mit läuft
            if (Debugger.IsAttached)
            {
                windows = 2;
            }

            if (Application.Current.Windows.Count > windows)
            {
                MessageBoxResult closingResult = MessageBox.Show("Möchten Sie das Programm wirklich beenden?", "Programm beenden", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (closingResult == MessageBoxResult.No)
                {
                    e.Cancel = true;

                    return;
                }
            }

            // speichern
            AccountListViewModel.Instance.saveAsync();
            
            Application.Current.Shutdown();

            base.OnClosing(e);
        }

        private void CompleteInboxButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<EmailViewModel> emails = new ObservableCollection<EmailViewModel>();
            foreach (AccountViewModel account in AccountListViewModel.Instance.Accounts)
            {
                foreach (EmailViewModel email in account.Emails)
                {
                    emails.Add(email);
                }
            }
            EmailsListView.ItemsSource = emails;

            // alle Buttons wieder weiß färben
            foreach (Button childenButton in AccountFoldersStackPanel.Children)
            {
                childenButton.Background = Brushes.White;
            }

            ((Button)sender).Background = Brushes.LightBlue;
        }
    }
}
