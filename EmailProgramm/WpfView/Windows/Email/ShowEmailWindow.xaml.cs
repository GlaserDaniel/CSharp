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
        public ShowEmailWindow()
        {
            InitializeComponent();
            Show();
        }

        public ShowEmailWindow(EmailViewModel email) : this()
        {
            email.IsRead = true;
            DataContext = email;

            String message = email.Message;

            Console.WriteLine("isHTML: " + email.IsHtml);

            // Damit Umlaute und Zeichen richtig dargestellt werden.
            message = "<!DOCTYPE html>\r<html><head><meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />" + message;

            message = message.Replace("\n", "<br>");

            //if (!email.IsHtml)
            //{
            //    message = "<pre style=\"font - family: Georgia, 'Times New Roman', serif; \">" + message + "</pre>";
            //}

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
                        // von https://msdn.microsoft.com/en-us/library/aa969773.aspx#Common_Dialogs
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
                    };

                    attachmentsStackPanel.Children.Add(button);
                }
            }
        }

        private void replyButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel replyEmail = new EmailViewModel();

            replyEmail.Sender = ((EmailViewModel)DataContext).Sender;
            replyEmail.Receivers = (((EmailViewModel)DataContext).Receivers);
            replyEmail.Subject = "RE: " + ((EmailViewModel)DataContext).Subject; // TODO könnte auch Re:
            replyEmail.Message = "\n\n" + ((EmailViewModel)DataContext).Sender + "\nschrieb am " + ((EmailViewModel)DataContext).DateTime + ": \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO jede Zeile ein >

            new SendEmailWindow(replyEmail);
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel forwardEmail = new EmailViewModel();

            //forwardEmail.Sender = ((EmailViewModel)DataContext).Sender;
            forwardEmail.Subject = "Fwd " + ((EmailViewModel)DataContext).Subject; // TODO könnte auch Fwd:
            forwardEmail.Message = "\n\nWeitergeleitete Email\nVon: " + ((EmailViewModel)DataContext).Sender + "\nDatum: " + ((EmailViewModel)DataContext).DateTime + "\nBetreff: " + ((EmailViewModel)DataContext).Subject + "\nEmpfänger: " + ((EmailViewModel)DataContext).ReceiversString + " \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO jede Zeile ein >

            new SendEmailWindow(forwardEmail);
        }

        private void replyAllButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
