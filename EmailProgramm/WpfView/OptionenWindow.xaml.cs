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
    /// Interaction logic for OptionenWindow.xaml
    /// </summary>
    public partial class OptionenWindow : Window
    {
        public OptionenWindow()
        {
            InitializeComponent();
            Show();
        }

        private void AppendSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO Einstellungen übernehmen
            Close();
        }

        private void AbortSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
