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
using Day012MidTerm.DataLayer;

namespace Day012MidTerm
{
    /// <summary>
    /// Interaction logic for PersonDlg.xaml
    /// </summary>
    public partial class PersonDlg : Window
    {
        public PersonDlg(Person person = null)
        {
            InitializeComponent();
            if (person == null)
            {
                btSave.Content = "Add";
            }
            else
            {
                btSave.Content = "Update";
                tbName.Text = person.Name;
                tbAge.Text = person.Age.ToString();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

            if (tbName.Text.Length < 1)
            {
                tbName.Focus();
                lbStatus.Content = "Please input the name";
                return;
            }


            if (tbAge.Text.Length < 1)
            {
                tbAge.Focus();
                lbStatus.Content = "Please input the age";
                return;
            }


            List<string> errList = new List<string>();

            string name = tbName.Text;
            if (name.Length < 2 || name.Length > 100 || name.Contains(";"))
            {
                errList.Add("Name must be 2-100 characters long, no semicolons");
            }

            if (!int.TryParse(tbAge.Text, out int age) || age > 150 || age < 0)
            {
                errList.Add("Age must be a number between 0-150");
            }



            if (errList.Count > 0)
            {
                string errMsg = "";
                foreach (string s in errList)
                {
                    errMsg += (s + "\n");
                }    
                MessageBox.Show($"Invalid input:\n{errMsg}");
                return;
            }

            this.DialogResult = true;
        }
    }
}
