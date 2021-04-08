using Day012MidTerm.DataLayer;
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


namespace Day012MidTerm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PersonPassportDbContext ctx;


        public MainWindow()
        {
            try
            {
                InitializeComponent();
                ctx = new PersonPassportDbContext(); // SystemEx
                ReloadRecords();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Failed to connect to database", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1); 
            }

        }


        public void ReloadRecords()
        {
            try
            {

                lvPeople.ItemsSource =ctx.People.OrderBy(p=>p.Id).ToList(); //SystemEx
              
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Failed to connect to database", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Add or update person base on lvPerople.SelectedItem
        private void miEdit_Save_Click(object sender, RoutedEventArgs e)
        {

            //Add new person
            if (lvPeople.SelectedItem == null)
            {
                PersonDlg dlg = new PersonDlg(null);
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    try
                    {
                        string name = dlg.tbName.Text;
                        int age = int.Parse(dlg.tbAge.Text);
                        Person person = new Person { Name = name, Age = age };
                        ctx.People.Add(person); //SystemEx
                        ctx.SaveChanges();
                        ReloadRecords();
                        tbStatus.Text = $"Added record";
                    }
                    catch (SystemException ex)
                    {

                        MessageBox.Show(ex.Message, "Failed to connect to database", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            //Update exsting person
            else
            {
                Person selPerson = lvPeople.SelectedItems.Cast<Person>().FirstOrDefault();
                int selId = selPerson.Id;
                PersonDlg dlg = new PersonDlg(selPerson);
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    try
                    {
                        Person person = ctx.People.Where(p => p.Id == selId).FirstOrDefault() as Person;//SystemEx

                        person.Name = dlg.tbName.Text;
                        person.Age = int.Parse(dlg.tbAge.Text);
                        ctx.SaveChanges();
                        ReloadRecords();
                        tbStatus.Text = $"Updated record";
                    }
                    catch (SystemException ex)
                    {

                        MessageBox.Show(ex.Message, "Failed to connect to database", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

        }

        //double click to create/update passport dialog
        private void lvPeople_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPeople.SelectedItem == null)
            {
                return;
            }

            Person selPerson = lvPeople.SelectedItems.Cast<Person>().FirstOrDefault();
            PassportDlg dlg = new PassportDlg(selPerson);
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string passportNo = dlg.tbPassportNo.Text;
                    byte[] photoArr = dlg.currImageByteArr;


                    //Create a passport if it doesn't exist
                    if (selPerson.Passport == null)
                    {
                        Passport newPassport = new Passport { PassportNo = passportNo, Photo = photoArr, Person = selPerson };
                        ctx.Passports.Add(newPassport);
                    }
                    else
                    { 
                    int selPassportId = selPerson.Passport.Id;
                    Passport passport = ctx.Passports.Where(p => p.Id == selPassportId).FirstOrDefault() as Passport;//SystemEx
                    passport.PassportNo = passportNo;
                    passport.Photo = photoArr;
                    }

                    ctx.SaveChanges();
                    ReloadRecords();
                }
                catch (SystemException ex)
                {

                    MessageBox.Show(ex.Message, "Failed to connect to database", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

       

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void lvPeople_LeftClick(object sender, MouseButtonEventArgs e)
        {
            lvPeople.SelectedItems.Clear();
        }
    }
}
