using Common;
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
        }

        private void loadData()
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel();

            DataContext = settingsViewModel;

            // Da es im XAML Code so nicht mehr funktioniert
            //ItemsSource = "{Binding Accounts[selectedAccountIndex].Emails}"
            if (((SettingsViewModel)DataContext).selectedAccountIndex != -1)
            {
                EmailsListView.ItemsSource = ((SettingsViewModel)DataContext).Accounts[((SettingsViewModel)DataContext).selectedAccountIndex].Emails;
            }
        }

        // TODO anderst lösen!
        void window_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("OptionWindow activated");
            loadData();
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SendEmail_Click");
            if (DataContext == null)
            {
                loadData();
            }
            if (DataContext != null && ((SettingsViewModel)DataContext).selectedAccountIndex >= 0)
            {
                new SendEmailWindow();
            }
            else
            {
                MessageBox.Show("Kein Account angelegt oder ausgewählt.", "Kein Account", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Optionen_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Optionen_Click");
            new SettingsWindow();
        }

        private void ReceiveEmails_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                loadData();
            }
            if (DataContext != null && ((SettingsViewModel)DataContext).selectedAccountIndex >= 0)
            {
                Account account = ((SettingsViewModel)DataContext).Accounts[((SettingsViewModel)DataContext).selectedAccountIndex];

                var progressHandler = new Progress<double>(value =>
                {
                    ProgressBar.Value = value;
                });

                Task t = new EmailViewModel().receiveEmails(account, progressHandler, Dispatcher);

                Task.Run(() =>
                {
                    t.Wait();
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ((SettingsViewModel)DataContext).appendSettings();
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
            new ShowEmailWindow((Email)((FrameworkElement)e.OriginalSource).DataContext);
        }

        private void DeleteEmails_Click(object sender, RoutedEventArgs e)
        {
            IList emailsToDelete = (IList)EmailsListView.SelectedItems;

            ObservableCollection<Email> emails = ((SettingsViewModel)DataContext).Accounts[((SettingsViewModel)DataContext).selectedAccountIndex].Emails;

            // TODO Funktioniert noch nicht richtig
            //foreach (var email in emailsToDelete)
            //{
            //    emails.Remove((Email)email);
            //}

            ((SettingsViewModel)DataContext).appendSettings();
        }

        private void EmailsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // TODO Funktioniert noch nicht richtig
                //foreach (ListViewItem listViewItem in ((ListView)sender).SelectedItems)
                //{
                //    listViewItem.Remove();
                //}
            }
        }

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
                MessageBoxResult closingResult = MessageBox.Show("Möchten Sie das Programm wirklich beenden?", "SMTP-Server fehlt", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (closingResult == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();

                    base.OnClosing(e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Application.Current.Shutdown();

                base.OnClosing(e);
            }
        }
    }
}
