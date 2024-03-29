﻿using Common.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int selectedAccountIndex = -1;

        /// <summary>
        /// Standard Konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            loadData();
            generateAccountFoldersButton();
        }

        private void loadData()
        {
            DataContext = AccountListViewModel.Instance;

            ObservableCollection<EmailViewModel> emails = new ObservableCollection<EmailViewModel>();
            foreach (AccountViewModel account in AccountListViewModel.Instance.Accounts)
            {
                foreach (EmailViewModel email in account.Emails)
                {
                    emails.Add(email);
                }
            }
            EmailsListView.ItemsSource = emails;

            sortEmailListView();

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

        private void sortEmailListView()
        {
            // von http://www.wpf-tutorial.com/listview-control/listview-sorting/
            // [
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(EmailsListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("DateTimeString", ListSortDirection.Descending));
            // ]
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
                        selectedAccountIndex = AccountListViewModel.Instance.Accounts.IndexOf(account);

                        // alle Buttons wieder weiß färben
                        foreach (Button childenButton in AccountFoldersStackPanel.Children)
                        {
                            childenButton.Background = Brushes.White;
                        }

                        EmailsListView.ItemsSource = account.Emails;
                        button.Background = Brushes.LightBlue;
                        sortEmailListView();
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
            new SettingsWindow();

            // Alle Account Buttons löschen
            AccountFoldersStackPanel.Children.RemoveRange(1, AccountFoldersStackPanel.Children.Count - 1);
            selectedAccountIndex = -1;
            CompleteInboxButton.Background = Brushes.LightBlue;
            // Account Buttons neu anlegen
            generateAccountFoldersButton();
        }

        private void ReceiveEmails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext == null)
                {
                    loadData();
                }
                if (DataContext != null)
                {
                    if (selectedAccountIndex >= 0)
                    {
                        // Für ausgewählten Account E-Mails abholen
                        AccountViewModel account = ((AccountListViewModel)DataContext).Accounts[selectedAccountIndex];

                        statusBar.Visibility = Visibility.Visible;

                        var progressHandler = new Progress<double>(value =>
                        {
                            ProgressBar.Value = value;
                        });

                        Task t = AccountListViewModel.Instance.receiveEmails(account, progressHandler, Dispatcher);

                        Task.Run(() =>
                        {
                            t.Wait();
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                ((AccountListViewModel)DataContext).saveAsync();
                                statusBar.Visibility = Visibility.Collapsed;
                            }));
                        });
                    }
                    else
                    {
                        // für alle Accounts E-Mails abholen
                        foreach (AccountViewModel account in ((AccountListViewModel)DataContext).Accounts)
                        {
                            var progressHandler = new Progress<double>(value =>
                            {
                                ProgressBar.Value = value;
                            });

                            Task t = AccountListViewModel.Instance.receiveEmails(account, progressHandler, Dispatcher);

                            Task.Run(() =>
                            {
                                t.Wait();
                                Dispatcher.BeginInvoke((Action)(() =>
                                {
                                    ((AccountListViewModel)DataContext).saveAsync();
                                }));
                            });
                        }
                    }
                    sortEmailListView();
                }
                else
                {
                    MessageBox.Show("Kein Account angelegt oder ausgewählt.", "Kein Account", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Beim Abholen der E-Mails ist es schief gelaufen.", "E-Mails abholen fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Ab und zu kommt da anscheind was anderes als ein EmailViewModel, deswegen abgefangen
                Console.WriteLine("selected Item by DoubleClick was no EmailViewModel");
            }
        }

        private void DeleteEmails_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult closingResult = MessageBox.Show("Möchten Sie die E-Mail wirklich löschen?", "E-Mail löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                MessageBoxResult closingResult = MessageBox.Show("Möchten Sie die E-Mail wirklich löschen?", "E-Mail löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
            selectedAccountIndex = -1;

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

            sortEmailListView();
        }
    }
}
