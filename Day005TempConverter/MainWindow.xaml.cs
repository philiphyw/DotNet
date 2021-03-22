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

namespace Day005TempConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblStatus != null)
            {
                lblStatus.Content = "";
            }
            
            ConvertTemp();
        }
        private void ScaleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (lblStatus !=null)
            {
                lblStatus.Content = "";
            }
            ConvertTemp();
        }


        private double GetConvertTemp(double temp, string fromUnit, string toUnit) 
        {
            double result;
            switch ((fromUnit + toUnit).ToUpper()) {
                case "FAHCEL":
                    result = (temp - 32) * 5 / 9;
                    break;
                case "KEVCEL":
                    result = temp - 273.15;
                    break;
                case "CELFAH":
                    result = temp * 9 / 5 + 32;
                    break;
                case "CELKEV":
                    result = temp + 273.15;
                    break;
                case "FAHKEV":
                    result = GetConvertTemp(GetConvertTemp(temp, "FAH", "CEL"),"CEL","KEV");
                    break;
                case "KEVFAH":
                    result = GetConvertTemp(GetConvertTemp(temp, "KEV", "CEL"), "CEL", "FAH");
                    break;
                default:
                    throw new ArgumentException("Invalid conversion units");

            }
            return Math.Round(result, 2);
        }

        private void ConvertTemp()
        {
            
           
            if (!tbInput.Text.Equals(""))
            {
                
                try
                {
                    //get input value
                    string InputTempStr = tbInput.Text;
                    double InputTemp = double.Parse(InputTempStr);//format ex


                    //get the input scale
                    RadioButton CheckedInputRB = spInputScales.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);
                    string InputUnit = "";
                    if (CheckedInputRB!=null)
                    {
                        InputUnit = CheckedInputRB.DataContext.ToString();
                    }

                    //get the output scale
                    RadioButton CheckedOutputRB = spOutputScales.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);
                    string OutputUnit = "";
                    if (CheckedOutputRB != null)
                    {
                        OutputUnit = CheckedOutputRB.DataContext.ToString();
                    }


                    //convert and show the output
                    double OutputTemp;

                    if (InputUnit.Equals(OutputUnit))
                    {
                        OutputTemp = InputTemp;
                    }
                    else {
                        OutputTemp = GetConvertTemp(InputTemp, InputUnit, OutputUnit);
                    }

                    tbOutput.Text = OutputTemp.ToString();


                }
                catch (Exception ex) when(ex is NullReferenceException || ex is FormatException)
                {
                    lblStatus.Content = ex.Message;
                }
            }

        }


    }
}
