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
using Common;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for OptionenWindow.xaml
    /// </summary>
    public partial class OptionenWindow : Window
    {
        SettingsController settingsController { get; set; }

        public OptionenWindow()
        {
            InitializeComponent();
            LoadData();
            Show();
        }

        private void LoadData()
        {
            settingsController = new SettingsController();

            DataContext = settingsController;
        }

        private void AppendSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO Einstellungen übernehmen
            settingsController.setAccount(userTextBox.Text, emailTextBox.Text, passwordBox.Password, serverTextBox.Text, int.Parse(portTextBox.Text));
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
