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
using System.Windows.Shapes;

namespace Day010FinalFlights
{
    /// <summary>
    /// Interaction logic for AddEditDialog.xaml
    /// </summary>
    public partial class AddEditDialog : Window
    {
        //private Flight _currentFlight = new Flight();
        public AddEditDialog(Flight flight)
        {
            InitializeComponent();
            comboFType.ItemsSource = Enum.GetValues(typeof(FType)).Cast<FType>().ToList();

            if (flight == null)
            { // adding
                btSave.Content = "Add";
            }
            else
            { // editing
                btSave.Content = "Update";
                dpOnDate.SelectedDate = flight.OnDate;
                tbFromCode.Text = flight.FromCode;
                tbToCode.Text = flight.ToCode;
                comboFType.Text = flight.FType.ToString();
                sliderPassengers.Value = flight.Passengers;

            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            string dateStr = $"{dpOnDate.SelectedDate:yyyy-MM-dd}";
            string fromCode =tbFromCode.Text;
            string toCode = tbToCode.Text;
            string fType = comboFType.Text;
            string passengers = sliderPassengers.Value.ToString();
            string dataString = $"{dateStr};{fromCode};{toCode};{fType};{passengers}";

            try
            {
                Flight flight = new Flight(dataString);
            }
            catch (InvalidDataException ex)
            {

                MessageBox.Show(this, "Error found on the data:\n" + ex.Message, "Error found", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true; // assing result and dismiss the dialog
        }
    }
}
