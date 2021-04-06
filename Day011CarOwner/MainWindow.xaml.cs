using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CsvHelper;
using Microsoft.Win32;

namespace Day011CarOwner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CarOwnerDbContext ctx;
        byte[] currOwnerImageByteArr;
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                //add owner name
                string name = tbName.Text;
                //add owner image
                byte[] imageByteArr = currOwnerImageByteArr;



                Owner owner = new Owner { Name = name, Image = imageByteArr };

                ////add owner cars
                //List<Car> cars = ctx.Cars.Include("Owner").Where(c => c.Owner == owner).ToList<Car>();


                ctx.Owners.Add(owner);
                ctx.SaveChanges();
                lvOwners.ItemsSource = ctx.Owners.OrderBy(r => r.Id).ToList<Owner>();//(from t in ctx.Owners orderby t.DueDate select t).ToList<Todo>();


                //Initialize input fileds for new record
                tbName.Text = "";
                tbImage.Visibility = Visibility.Visible;
                imagePreview.Source = null;
                currOwnerImageByteArr = null;

                lbStatus.Content = $"Added record: {owner.Id};{owner.Name}";
            }
            catch (InvalidDataException ex)
            {

                MessageBox.Show(this, "Error found on the data:\n" + ex.Message, "Error found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }



        public MainWindow()
        {
            try
            {
                InitializeComponent();
                ctx = new CarOwnerDbContext();
                lvOwners.ItemsSource = ctx.Owners.OrderBy(s => s.Id).ToList<Owner>();
                btUpdate.IsEnabled = false;
                btDelete.IsEnabled = false;
            }
            catch (SystemException ex)
            {

                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            
        }




        private void ManageCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvOwners.SelectedItem == null)
            {
                return;
            }

            Owner selOwner = lvOwners.SelectedItem as Owner;
            int selId = selOwner.Id;

            ManageCarDlg dlg = new ManageCarDlg(selOwner);
            dlg.ShowDialog();

            //refresh the list to show cars number after managing cars
            lvOwners.ItemsSource = ctx.Owners.OrderBy(s => s.Id).ToList<Owner>();
        }

        private void lvOwnersSelection_Change(object sender, SelectionChangedEventArgs e)
        {
            imagePreview.Source = null;
            tbImage.Visibility = Visibility.Visible;


            if (lvOwners.SelectedItem == null)
            {
                btUpdate.IsEnabled = false;
                btDelete.IsEnabled = false;
                return;
            }

            btUpdate.IsEnabled = true;
            btDelete.IsEnabled = true;
            Owner selOwner = lvOwners.SelectedItem as Owner;
            tbName.Text = selOwner.Name;
            lblId.Content = selOwner.Id;

            if (selOwner.Image != null)
            {
                using (MemoryStream stream = new MemoryStream(selOwner.Image))
                {

                    imagePreview.Source = BitmapFrame.Create(stream,
                                                     BitmapCreateOptions.None,
                                                     BitmapCacheOption.OnLoad);

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
                    currOwnerImageByteArr = File.ReadAllBytes(dlg.FileName);
                    tbImage.Visibility = Visibility.Hidden; // hide the text box
                                                            //System.Drawing.Image bitmap  = byteArrayToImage(currOwnerImage); // ex: SystemException


                    
                    using (MemoryStream stream = new MemoryStream(currOwnerImageByteArr))
                    {

                        imagePreview.Source = BitmapFrame.Create(stream,
                                                         BitmapCreateOptions.None,
                                                         BitmapCacheOption.OnLoad);

                    }


                }
                catch (SystemException ex)
                {
                    MessageBox.Show(ex.Message, "File reading failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        //update owner
        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvOwners.SelectedItem == null)
            {
                return;
            }

            Owner selOwner = lvOwners.SelectedItem as Owner;
            int selId = selOwner.Id;

            Owner udOwner = ctx.Owners.Where(o => o.Id == selId).FirstOrDefault();


            if (tbName.Text.Length>0)
            {
                udOwner.Name = tbName.Text;
            }          

            if (imagePreview.Source == null)
            {
                udOwner.Image = null;
            }else if(currOwnerImageByteArr != null)
            {
                udOwner.Image = currOwnerImageByteArr;
            }

            ctx.SaveChanges();
            lbStatus.Content = $"Updated record: {udOwner.Id};{udOwner.Name}";
        }


        //delete owner
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOwners.SelectedItem == null)
            {
                return;
            }

            Owner selOwner = lvOwners.SelectedItem as Owner;
            int selId = selOwner.Id;

            Owner dlOwner = ctx.Owners.Where(o => o.Id == selId).FirstOrDefault();

            var returnVal = MessageBox.Show("Do you want to remove this record?\n" + $"{dlOwner.Id}{dlOwner.Name}", "Deleting record", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (returnVal == MessageBoxResult.OK)
            {
                //remove the cars under the to-be-deleted owners
                List<Car> dlCarList = ctx.Cars.Where(c => c.Owner.Id == dlOwner.Id).ToList<Car>();
                foreach (Car car in dlCarList)
                {
                    ctx.Cars.Remove(car);
                    ctx.SaveChanges();
                    //Where(c => c.Id == car.Id);
                }

                ctx.Owners.Remove(dlOwner);
                ctx.SaveChanges();
                lblId.Content = "-";
                tbName.Text = "";
                tbImage.Visibility = Visibility.Visible;
                imagePreview.Source = null;
                lvOwners.ItemsSource = ctx.Owners.OrderBy(o => o.Id).ToList<Owner>();
                lbStatus.Content = $"Deleted record:{selOwner.Id};{selOwner.Name}";
            }

        }


        private void cmOwnerImageRemove_Click(object sender, RoutedEventArgs e)
        {
            //Remove picture
            currOwnerImageByteArr = null;
            tbImage.Visibility = Visibility.Visible;
            imagePreview.Source = null;
        }


        private void btImage_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // show context menu cmOwnerImage
            ContextMenu cm = this.FindResource("cmOwnerImage") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        //Context menu to export selected owner: id, name, list of car. but NOT image(byte[]) to a csv file 
        private void cmLvOwnersExportSelection_Click(object sender, RoutedEventArgs e)
        {
            if (lvOwners.SelectedItem == null)
            {
                lbStatus.Content = "No records are selected for exporting";
                return;
            }

            
           var selOwnerList = lvOwners.SelectedItems.Cast<Owner>().Select(o => o.toDataString()).ToList();
            List<string> dataStringList = new List<string>();    
            using (var writer = new StreamWriter(@"..\\..\\selected owners.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {

                
                foreach (string ds in selOwnerList)
                {
                    csv.WriteField(ds);
                    csv.NextRecord();
                }

                writer.Flush();
            }
            lbStatus.Content = $"Exported {selOwnerList.Count} record(s)";
        }
        //right-click on the lvOwners to show context menu cmLvOwners
        private void lvOwners_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmLvOwners") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
    }
}
