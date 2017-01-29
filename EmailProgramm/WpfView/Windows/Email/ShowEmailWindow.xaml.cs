using Common.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for ShowEmailWindow.xaml
    /// </summary>
    public partial class ShowEmailWindow : Window
    {
        /// <summary>
        /// Konstruktor dem eine E-Mail übergeben wird und die Daten dieser im Fenster setzt
        /// </summary>
        /// <param name="email"></param>
        public ShowEmailWindow(EmailViewModel email)
        {
            InitializeComponent();
            email.IsRead = true;
            DataContext = email;

            Title = email.Subject;

            String message = email.Message;

            Console.WriteLine("isHTML: " + email.IsHtml);

            // Damit Umlaute und Zeichen richtig dargestellt werden.
            message = "<!DOCTYPE html>\r<html><head><meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />" + message;

            message = message.Replace("\n", "<br>");

            Console.WriteLine("Message: " + message);

            if (!String.IsNullOrEmpty(message))
            {
                messageWebBrowser.NavigateToString(message);
            }

            if (email.Attachments.Count > 0)
            {
                attachmentsStackPanel.Visibility = Visibility.Visible;

                foreach (var attachment in email.Attachments)
                {
                    Button button = new Button();
                    button.Height = 23;
                    button.Padding = new System.Windows.Thickness(5, 0, 5, 0);
                    button.Background = Brushes.White;
                    button.Content = attachment;
                    button.Click += (source, e) =>
                    {
                        // von https://msdn.microsoft.com/en-us/library/aa969773.aspx#Common_Dialogs und angepasst
                        // [
                        // Configure save file dialog box
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.FileName = attachment; // Default file name
                        dlg.DefaultExt = ""; // Default file extension
                        dlg.Filter = "All Files|*.*"; // Filter files by extension

                        // Show save file dialog box
                        Nullable<bool> result = dlg.ShowDialog();

                        // Process save file dialog box results
                        if (result == true)
                        {
                            // Save document
                            string filename = dlg.FileName;

                            using (var stream = File.Create(filename))
                            {
                                // TODO speichern der Datei
                                // Attachment muss noch was anderes sein. Im Moment nur der Dateiname.

                                MessageBox.Show("Speichern geht noch nicht!");
                            }
                        }
                        // ]
                    };

                    attachmentsStackPanel.Children.Add(button);
                }
            }
            Show();
        }

        private void replyButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel replyEmail = new EmailViewModel();

            replyEmail.Sender = ((EmailViewModel)DataContext).Sender;
            replyEmail.Subject = "Re: " + ((EmailViewModel)DataContext).Subject;
            replyEmail.Message = "\n\n" + ((EmailViewModel)DataContext).Sender + "\nschrieb am " + ((EmailViewModel)DataContext).DateTime + ": \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO jede Zeile ein >

            new SendEmailWindow(replyEmail);
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel forwardEmail = new EmailViewModel();

            forwardEmail.Subject = "Fwd: " + ((EmailViewModel)DataContext).Subject;
            forwardEmail.Message = "\n\nWeitergeleitete E-Mail\nVon: " + ((EmailViewModel)DataContext).Sender + "\nDatum: " + ((EmailViewModel)DataContext).DateTime + "\nBetreff: " + ((EmailViewModel)DataContext).Subject + "\nEmpfänger: " + ((EmailViewModel)DataContext).ReceiversString + " \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO jede Zeile ein >

            new SendEmailWindow(forwardEmail);
        }

        private void replyAllButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel replyAllEmail = new EmailViewModel();

            replyAllEmail.Sender = ((EmailViewModel)DataContext).Sender;
            replyAllEmail.Receivers = (((EmailViewModel)DataContext).Receivers);
            // Die eigene E-Mail Adresse raus löschen
            replyAllEmail.Receivers.Remove(AccountListViewModel.Instance.Accounts[((EmailViewModel)DataContext).AccountIndex].Email);
            replyAllEmail.Subject = "Re: " + ((EmailViewModel)DataContext).Subject;
            replyAllEmail.Message = "\n\n" + ((EmailViewModel)DataContext).Sender + "\nschrieb am " + ((EmailViewModel)DataContext).DateTime + ": \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO jede Zeile ein >

            new SendEmailWindow(replyAllEmail);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Willst du die E-Mail wirklich löschen?", "E-Mail löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close();
                AccountListViewModel.Instance.Accounts[((EmailViewModel)DataContext).AccountIndex].DeleteSelectedCommandExecute((EmailViewModel)DataContext);
            }
        }

        private void unreadButton_Click(object sender, RoutedEventArgs e)
        {
            ((EmailViewModel)DataContext).IsRead = false;
        }
    }
}
