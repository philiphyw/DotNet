using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Day010PeopleGuiEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PeopleDBContext ctx;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                ctx = new PeopleDBContext();
               
                lvPeople.ItemsSource = (from p in ctx.People select p).ToList<Person>();
                lvPeople.Items.Refresh();

            }
            catch (SystemException ex)//catch-all for EF ,SQL and other exceptions
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(this, "Database connection failed" + ex.Message);
                Environment.Exit(1);
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (!int.TryParse(tbAge.Text,out int age))
            {
                MessageBox.Show(this, "Invalid input on the Age field");
                return;
            }
            try
            {
                Person person = new Person { Name = name, Age = age };
                ctx.People.Add(person);
                ctx.SaveChanges();
                tbName.Text = "";
                tbAge.Text = "0";
                lvPeople.ItemsSource = (from p in ctx.People select p).ToList<Person>();
                lvPeople.Items.Refresh();
            }
            catch(SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        private void UDPerson(string command)
        {
            lblStatus.Content = "";

            if (lvPeople.SelectedItem == null)
            {
                return;
            }
            try
            {
                Person selPerson = (Person)lvPeople.SelectedItem;
                int selId = selPerson.Id;

                Person updPerson = (from p in ctx.People where p.Id == selId select p).FirstOrDefault<Person>();

                if (updPerson == null)
                {
                    MessageBox.Show($"Can not found this {selPerson.Id};{selPerson.Name};{selPerson.Age} in the database");
                    return;
                }

                switch (command.ToUpper())
                {
                    case "DELETE":

                        var returnVal = MessageBox.Show($"Please confirm you want to delete this record:\n{ updPerson.Id};{ updPerson.Name};{ updPerson.Age}                    ","Deleting record", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (returnVal==MessageBoxResult.OK)
                        {
                            ctx.People.Remove(updPerson);
                           lblStatus.Content = ($"{updPerson.Id};{updPerson.Name};{updPerson.Age} has been deleted");
                        }


                        
                        break;
                    case "UPDATE":
                        updPerson.Name = tbName.Text;
                        if (!int.TryParse(tbAge.Text, out int age))
                        {
                            MessageBox.Show("Invalid input on the Age field");
                            return;
                        }
                        updPerson.Age = age;

                        lblStatus.Content = ($"{updPerson.Id};{updPerson.Name};{updPerson.Age} has been updated");
                        break;                     
                    default:
                        MessageBox.Show("Invalid command");
                        return;


                }
                

                ctx.SaveChanges();
                lvPeople.ItemsSource = (from p in ctx.People select p).ToList<Person>();
                lvPeople.Items.Refresh();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

        }



        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            UDPerson("update");

        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            UDPerson("delete");

        }




        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

       

        private void lvPeopleSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (lvPeople.SelectedItem == null)
            {
                return;
            }
            try
            {
                Person selPerson = (Person)lvPeople.SelectedItem;
                lblId.Content = selPerson.Id;
                tbName.Text = selPerson.Name;
                tbAge.Text = selPerson.Age.ToString();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
    }


}
