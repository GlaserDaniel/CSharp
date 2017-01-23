using Common.ViewModel;
using System;
using System.Windows;

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

            if (!email.IsHtml)
            {
                // Damit Umlaute und Zeichen richtig dargestellt werden.
                //if (message.Length >= 9 && !message.Substring(0, 9).Equals("<!DOCTYPE"))
                //{
                    message = "<!DOCTYPE html>\r\n<html><head><meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />" + message;
                //}

                message = "<pre>" + message + "</pre>";
            }

            Console.WriteLine("Message: " + message);

            if (!String.IsNullOrEmpty(message))
            {
                messageWebBrowser.NavigateToString(message);
            }

            // TODO HTML Text richtig darstellen

            //Windows.Data.Html.HtmlUtilities.ConvertToText();

            // TODO wenn HTML
            //// xaml Code: <TextBox x:Name="messageTextBox" Text="{Binding message}" Margin="10" AcceptsReturn="True" TextWrapping="Wrap" HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Auto"/>
            //TextBox messageTextBox = new TextBox();
            ////messageTextBox.Text = "{Binding message}";
            //messageTextBox.Set = DataContext;
            //messageTextBox.Margin = new System.Windows.Thickness(10);
            //messageTextBox.AcceptsReturn = true;
            //messageTextBox.TextWrapping = TextWrapping.Wrap;
            //messageTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            //messageTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            //MainDockPanel.Children.Add(messageTextBox);

            // oder so 
            //MainDockPanel.Children.Remove(messageTextBox);

            /*
            < Grid >
              < WebBrowser Name = "wb1" />
 
              </ Grid >
             </ Window >
             Der CodeBehind dazu:

            Class MainWindow

             Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
              Me.wb1.NavigateToString("<html>Die <i>Erde</i> ist <b>rund</b></html>")
             End Sub
             */
        }

        private void replyButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel replyEmail = new EmailViewModel();

            replyEmail.Sender = ((EmailViewModel)DataContext).Sender;
            replyEmail.Receivers = (((EmailViewModel)DataContext).Receivers);
            replyEmail.Subject = "Antwort auf " + ((EmailViewModel)DataContext).Subject; // TODO könnte auch Re:
            replyEmail.Message = "\n\nAlte Email: \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO Mehr Informationen

            new SendEmailWindow(replyEmail);
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            EmailViewModel forwardEmail = new EmailViewModel();

            //forwardEmail.Sender = ((EmailViewModel)DataContext).Sender;
            forwardEmail.Subject = "Weiterleitung von " + ((EmailViewModel)DataContext).Subject; // TODO könnte auch Fwd:
            forwardEmail.Message = "\n\nWeitergeleitete Email von " + ((EmailViewModel)DataContext).Sender + ": \n{\n" + ((EmailViewModel)DataContext).Message + "\n}"; // TODO Mehr Informationen

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
