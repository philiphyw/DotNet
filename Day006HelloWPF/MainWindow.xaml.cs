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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Day006HelloWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SayHelloLabelButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length>0)
            {
                string UserName = tbName.Text.ToString();
                lblOutput.Content = $"Hello there, {UserName}";
            }
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblOutput.Content = "";
        }

        private void SayHelloMsgBoxButton_Click(object sender, RoutedEventArgs e)
        {
            string UserName = tbName.Text.ToString();
            if (UserName == "")
            {
                MessageBox.Show(this, "Please enter a name", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            MessageBox.Show(this, $"Hello, {UserName}. How are you?","Welcome", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
