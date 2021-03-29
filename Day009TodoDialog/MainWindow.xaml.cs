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

namespace Day009TodoDialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Todo> TodoList = new List<Todo>();
        public MainWindow()
        {
            InitializeComponent();
            lvTodos.ItemsSource = TodoList;
        }

        private void miEditAddUpdate_Click(object sender, RoutedEventArgs e)
        {

            AddEditDlg dlg = new AddEditDlg();
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {

            }
        }
    }
}
