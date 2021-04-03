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

namespace Day010TodoDlgEF
{
    /// <summary>
    /// Interaction logic for AddEditDlg.xaml
    /// </summary>
    public partial class AddEditDlg : Window
    {
        public AddEditDlg(Todo todo = null)
        {
            InitializeComponent();

            comboEStatus.ItemsSource = Enum.GetValues(typeof(EStatus)).Cast<EStatus>().ToList();

            if (todo == null)
            {
                btSave.Content = "Add";
            }
            else
            { 
                btSave.Content = "Update";
                tbTask.Text = todo.Task;
                sliderDifficulty.Value = todo.Difficulty;
                dpDueDate.SelectedDate = todo.DueDate;
                comboEStatus.Text = todo.TStatus.ToString();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

            if (tbTask.Text.Length < 1)
            {
                tbTask.Focus();
                return;
            }


            if (dpDueDate.SelectedDate == null)
            {
                dpDueDate.Focus();
                return;
            }


            this.DialogResult = true;
        }
    }
}
