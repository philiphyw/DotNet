using System;
using System.Collections.Generic;
using System.IO;
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

namespace Day010FinalFlights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Flight> FlightList = new List<Flight>();
        const string DataFileName = @"..\..\flights.txt";


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDataToFile(DataFileName, FlightList);
        }


        private void SaveDataToFile(string fileName, List<Flight> flightToBeSaved)
        {
            try
            {
                List<string> linesList = new List<string>();
                foreach (Flight f in flightToBeSaved)
                {
                    linesList.Add(f.ToDataString());
                }
                File.WriteAllLines(fileName, linesList);
            }
            catch (Exception ex) when (ex is IOException || ex is SystemException)
            {
                MessageBox.Show(this, ex.Message, "File IO error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadDataFromFile()
        {
            try
            {
                if (!File.Exists(DataFileName)) return; // it's ok to be null for the first time the program runs
                string[] linesList = File.ReadAllLines(DataFileName);
                foreach (string line in linesList)
                {
                    try
                    {
                        Flight flight = new Flight(line); // ex InvalidDataException
                        FlightList.Add(flight);
                    }
                    catch (InvalidDataException ex)
                    {
                        MessageBox.Show(this, "Ignoring invalid line.\n" + ex.Message, "Data String Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                lvFlights.Items.Refresh();

            }
            catch (Exception ex) when (ex is IOException || ex is SystemException)
            {
                MessageBox.Show(this, ex.Message, "File IO error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        //Event handlers
        private void SaveSelection_Click(object sender, RoutedEventArgs e)
        {
            if (lvFlights.SelectedItem == null)
            {
                MessageBox.Show(this, "Please select record(s) to save", "No Selected Records", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Flight> selFlightList = lvFlights.SelectedItems.Cast<Flight>().ToList();

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.TXT";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;


            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName == "")
            {
                return;
            }


            string TargetFileName = saveFileDialog.FileName;
            SaveDataToFile(TargetFileName, selFlightList);
            lvFlights.SelectedItems.Clear();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddEditDialog dlg = new AddEditDialog(null);
            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {
                string dateStr = $"{dlg.dpOnDate.SelectedDate:yyyy-MM-dd}";
                string fromCode = dlg.tbFromCode.Text;
                string toCode = dlg.tbToCode.Text;
                string fType = dlg.comboFType.Text;
                string passengers = dlg.sliderPassengers.Value.ToString();
                string dataString = $"{dateStr};{fromCode};{toCode};{fType};{passengers}";

                try
                {
                    FlightList.Add(new Flight(dataString));
                    lvFlights.Items.Refresh();
                    tbStatus.Text = FlightList.Count.ToString();
                }
                catch (InvalidDataException ex)
                {

                    MessageBox.Show(this, "Error found on the data:\n" + ex.Message, "Error found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void lvFlights_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selFlight = (Flight)lvFlights.SelectedItem;

            if (selFlight == null)
            {
                tbStatus.Text = "Total Flights: " + FlightList.Count.ToString();
                return;
            }
            else
            {
                int selIndex = lvFlights.SelectedIndex;
                AddEditDialog dlg = new AddEditDialog(selFlight);
                dlg.Owner = this;

                if (dlg.ShowDialog() == true)
                {
                    string dateStr = $"{dlg.dpOnDate.SelectedDate:yyyy-MM-dd}";
                    string fromCode = dlg.tbFromCode.Text;
                    string toCode = dlg.tbToCode.Text;
                    string fType = dlg.comboFType.Text;
                    string passengers = dlg.sliderPassengers.Value.ToString();
                    string dataString = $"{dateStr};{fromCode};{toCode};{fType};{passengers}";

                    try
                    {
                        selFlight = new Flight(dataString);
                        FlightList[selIndex] = selFlight;
                        lvFlights.SelectedItems.Clear();
                        lvFlights.Items.Refresh();
                        tbStatus.Text = "Total Flights: " + FlightList.Count.ToString();
                    }
                    catch (InvalidDataException ex)
                    {

                        MessageBox.Show(this, "Error found on the data:\n" + ex.Message, "Error found", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }

        }


        private void lvFlights_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Invoke ContextMenu
            ContextMenu cm = this.FindResource("cmLvFlights") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void cmLvFlights_DeleteClick(object sender, RoutedEventArgs e)
        {

            if (lvFlights.SelectedItem == null) return;
            Flight selFlight = (Flight)lvFlights.SelectedItem;
            int selIndex = lvFlights.SelectedIndex;

            string msg = "Do you want to delete following record?\n" + selFlight.ToString();
            MessageBoxResult result = MessageBox.Show(msg, "Deleting a flight", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                FlightList.RemoveAt(selIndex);
                lvFlights.Items.Refresh();
                tbStatus.Text = "Selected flight has been deleted.";

            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        public MainWindow()
        {
            InitializeComponent();
            FlightList = new List<Flight>();
            lvFlights.ItemsSource = FlightList;
            LoadDataFromFile();
            FlightList.Sort((f1, f2) => f1.OnDate.CompareTo(f2.OnDate));
            tbStatus.Text = "Total Flights: " + FlightList.Count.ToString();

        }
    }

}
