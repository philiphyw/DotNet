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

namespace Day009FriendCusDlg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> friendList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            lvFriendList.ItemsSource = friendList;
        }

        private void miEditAdd_Click(object sender, RoutedEventArgs e)
        {
           
            AddEditDialog dlg = new AddEditDialog();
            dlg.Owner = this;

            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                string name = dlg.tbName.Text;
                friendList.Add(name);
                lvFriendList.Items.Refresh();
            }

        }
    }
}
