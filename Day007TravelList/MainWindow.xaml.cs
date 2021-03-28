using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Day007TravelList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> DataStringList = new List<string>();
        private List<Trip> TripList = new List<Trip>();
        private const string DATA_SOURCE = "..\\..\\DataString.txt";
        private bool isModified = false;

        private void SaveDataToFile()
        {

            

            if (lbTripList.SelectedItem == null)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Please select a trip to save", "Confirmation");
                return;
            }





            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.TXT";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;


            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName == "")
            {
                return;
            }


            List<string> SelectedDataString = new List<string>();

            foreach (Trip item in lbTripList.SelectedItems)
            {
                string DataString = item.ToDataString();
                SelectedDataString.Add(DataString);
            }
           

            File.WriteAllLines(saveFileDialog.FileName, SelectedDataString);

            System.Windows.MessageBox.Show($"Saved data to {saveFileDialog.FileName}", "Data Saved");
            lblStatus.Content = $"{SelectedDataString.Count} record(s) saved";

            lbTripList.SelectedItems.Clear();

        }


        private void LoadDataFromFile()
        {

            try
            {
                DataStringList = File.ReadAllLines(DATA_SOURCE).ToList();

                foreach (string dataString in DataStringList)
                {
                    TripList.Add(new Trip(dataString));
                }

                lbTripList.Items.Refresh();
                lblStatus.Content = DataStringList.Count();
            }
            catch (Exception ex) when (ex is InvalidDataException || ex is IOException) 
            {
                System.Windows.MessageBox.Show(ex.Message, "Error found");
            }

           
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadDataFromFile();
            lbTripList.ItemsSource = TripList;
      
        }

        private void btSaveSelected_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToFile();
        }

        private void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = tbName.Text;
                string passport = tbPassport.Text;
                string destination = tbDestination.Text;
                DateTime departure = dpDeparture.SelectedDate.Value;
                DateTime @return = dpReturn.SelectedDate.Value;

                Trip trip = new Trip( $"{name};{passport};{destination};{departure.Date:d};{@return.Date:d}");
                TripList.Add(trip);
                lbTripList.Items.Refresh();//inform the lbTripList has been changed
                isModified = true;
            }
            catch (Exception ex) when(ex is InvalidOperationException || ex is InvalidDataException)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }




        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isModified)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to save the changes?", "Unsave change", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (result == MessageBoxResult.No)
                {
   
                }
                else if (result == MessageBoxResult.Yes)
                {
                    DataStringList = TripList.Select(s => s.ToDataString()).ToList();
                    File.WriteAllLines(DATA_SOURCE, DataStringList);
                }
            }
        }
    }
}
