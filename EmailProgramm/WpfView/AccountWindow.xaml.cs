﻿using System;
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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private SettingsController settingsController;
        private Account selectedAccountToRemove;

        public AccountWindow()
        {
            InitializeComponent();
            this.selectedAccountToRemove = null;
            Console.WriteLine("Constr");
            Show();
        }
        
        public AccountWindow(SettingsController settingsController) : this()
        {
            Console.WriteLine("Constr mit Con");
            this.settingsController = settingsController;
            Title = "Account hinzufügen";

            Show();
        }

        public AccountWindow(SettingsController settingsController, Account selectedAccountToRemove) : this()
        {
            Console.WriteLine("Constr mit Account");
            this.settingsController = settingsController;
            this.selectedAccountToRemove = selectedAccountToRemove;
            Title = "Account bearbeiten";

            DataContext = this.selectedAccountToRemove;

            Show();
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccountToRemove != null)
            {
                settingsController.removeAccount(selectedAccountToRemove);
            }
            // TODO
            // string user, string email, string password, bool useImap, string imapPop3Server, int imapPop3Port, string smtpServer, int smtpPort
            settingsController.addAccount(userTextBox.Text, emailTextBox.Text, passwordBox.Password, (bool)useImapCheckBox.IsChecked, imapPop3ServerTextBox.Text, int.Parse(imapPop3PortTextBox.Text), smtpServerTextBox.Text, int.Parse(smtpPortTextBox.Text));
            
            Close();
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}