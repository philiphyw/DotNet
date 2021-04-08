using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Day012MidTerm.DataLayer;
using Microsoft.Win32;

namespace Day012MidTerm
{
    /// <summary>
    /// Interaction logic for PassportDlg.xaml
    /// </summary>
    public partial class PassportDlg : Window
    {
        PersonPassportDbContext ctx;
        public byte[] currImageByteArr;
        Person currentPerson;

        public PassportDlg(Person person = null)
        {
            InitializeComponent();

            if (person == null)
            {
                btSave.Content = "Add";
                currImageByteArr = null;
            }
            else
            {
                currentPerson = person;
                btSave.Content = "Update";
                lbName.Content = person.Name;
                if (person.Passport !=null)
                {
                    tbPassportNo.Text = person.Passport.PassportNo;
                    currImageByteArr = person.Passport.Photo;
                    tbImage.Visibility = Visibility.Hidden; // hide the text box
                    using (MemoryStream stream = new MemoryStream(currImageByteArr))
                    {
                        imagePreview.Source = BitmapFrame.Create(stream,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }
                }
            }
        }

        private void btImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            //openFileDialog.InitialDirectory = "..\\..\\";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dlg.FilterIndex = 2;
            dlg.RestoreDirectory = true;


            if (dlg.ShowDialog() == true)
            {
                try
                {
                    currImageByteArr = File.ReadAllBytes(dlg.FileName);
                    tbImage.Visibility = Visibility.Hidden; // hide the text box
                      using (MemoryStream stream = new MemoryStream(currImageByteArr))
                    {
                        imagePreview.Source = BitmapFrame.Create(stream,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);
                    }

             }
                catch (SystemException ex)
                {
                    MessageBox.Show(ex.Message, "Failed to read the file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

            if (tbPassportNo.Text.Length<1)
            {
                tbPassportNo.Focus();
                lbStatus.Content = "Please input the passport number";
                return;
            }

            if (imagePreview.Source == null)
            {
                btImage.Focus();
                lbStatus.Content = "Please select a photo";
                return;
            }



            List<string> errList = new List<string>();

            string passportNo = tbPassportNo.Text;
            if (!Regex.IsMatch(passportNo, @"^[A-Z]{2}[0-9]{6}$"))
            {
                errList.Add("Passport number must be 2 uppercase letters follow by 6 numbers");
            }

            //until we can use fluent api to control the unique PassportNo, use a query to check if it's unique in the system
            ctx = new PersonPassportDbContext();
            bool isDuplicatePN = false;
            List<Person> matchedList = ctx.People.Where(p => p.Passport.PassportNo.Equals(passportNo)).ToList();
            if (currentPerson.Passport == null)
            {
                if (matchedList.Count>0)
                {
                    isDuplicatePN = true;
                }
            }
            else
            {
                if (matchedList.Count > 1)
                {
                    isDuplicatePN = true;
                }
            }

            if (isDuplicatePN==true)
            {
                errList.Add("Passport number already exists for another person, please input a new one");
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
