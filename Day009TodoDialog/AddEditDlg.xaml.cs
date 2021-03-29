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

namespace Day009TodoDialog
{
    /// <summary>
    /// Interaction logic for AddEditDlg.xaml
    /// </summary>
    public partial class AddEditDlg : Window
    {
        public AddEditDlg(Todo todo = null)
        {
            InitializeComponent();

            if (todo == null)
            {
                btSave.Content = "Add";
            }
            else
            {
                btSave.Content = "Update";
            }
        }



    }
}
