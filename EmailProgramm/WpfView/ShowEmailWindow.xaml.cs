using Common;
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

        public ShowEmailWindow(Email email) : this()
        {
            DataContext = email;

            String message = email.message;

            //message.Replace("ä", "&auml;");
            //message.Replace("ü", "&uuml;");
            //message.Replace("ö", "&ouml;");
            //message.Replace("Ä", "&Auml;");
            //message.Replace("Ü", "&Uuml;");
            //message.Replace("Ö", "&Ouml;");
            //message.Replace("ß", "&szlig;");

            //message.Replace("€", "&euro;");
            //message.Replace("&", "&amp;");
            //message.Replace("<", "&lt");
            //message.Replace(">", "&gt");
            //message.Replace("„", "&quot;");
            //message.Replace("©", "&copy;");
            //message.Replace("•", "&bull;");
            //message.Replace("™", "&trade;");
            //message.Replace("®", "&reg;");
            //message.Replace("§", "&sect;");

            // Damit Umlaute und Zeichen richtig dargestellt werden.
            if (!message.Substring(0,9).Equals("<!DOCTYPE"))
            {
                message = "<!DOCTYPE html>\r\n<html><head><meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />" + message;
            }

            messageWebBrowser.NavigateToString(message);

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
            Email replyEmail = new Email();

            replyEmail.sender = ((Email)DataContext).sender;
            replyEmail.subject = "Antwort auf " + ((Email)DataContext).subject; // TODO könnte auch Re:
            replyEmail.message = "\n\nAlte Email: \n{\n" + ((Email)DataContext).message + "\n}"; // TODO Mehr Informationen

            new SendEmailWindow(replyEmail);
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            Email forwardEmail = new Email();

            forwardEmail.sender = ((Email)DataContext).sender;
            forwardEmail.subject = "Weiterleitung von " + ((Email)DataContext).subject; // TODO könnte auch Fwd:
            forwardEmail.message = "\n\nWeitergeleitete Email: \n{\n" + ((Email)DataContext).message + "\n}"; // TODO Mehr Informationen

            new SendEmailWindow(forwardEmail);
        }
    }
}
