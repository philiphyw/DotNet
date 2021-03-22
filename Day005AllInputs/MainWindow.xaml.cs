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

namespace Day005AllInputs
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

        private void btRegisterMe_Click(object sender, RoutedEventArgs e)
        {
            List<string> errList = new List<string>();
            //check name
            string Name = tbName.Text;
            if (Name == "")
            {
                errList.Add("Please enter your name");
            }


            //get age
            
            string AgeStr = "";
            List<RadioButton> rbList = spAges.Children.OfType<RadioButton>().ToList();
            for (int i = 0; i < rbList.Count; i++)
            {
                if (rbList[i].IsChecked ==true)
                {
                    AgeStr = rbList[i].Content.ToString();
                }
            }


            //Check continent

            string Continent = "";

            if (comboContinent.SelectedItem == null)
            {
                errList.Add("Please select the continent of residence");
            }
            else
            {
                Continent = comboContinent.Text;
            }
            //check select pets
            List<string> petList = new List<string>();
            if (cbCat.IsChecked == true)
            {
                petList.Add(cbCat.Content.ToString());
            }
            if (cbDog.IsChecked == true)
            {
                petList.Add(cbDog.Content.ToString());
            }
            if (cbOther.IsChecked == true)
            {
                petList.Add(cbOther.Content.ToString());
            }

            string Pets = "";
            if (petList.Count>0)
            {
                for (int i = 0; i < petList.Count; i++)
                {
                    Pets += petList[i];
                }
            }

            //get temp
            double PreferredTemp = sliderPerferredTemp.Value;
            string PreferredTempStr = Math.Round(PreferredTemp, 0).ToString();

            if (errList.Count>0)
            {
                string errStr="";
                errList.ForEach(er => errStr+=er.ToString()+"\n");
                MessageBox.Show("Invalid Input(s):\n" + errStr);
                return;
            }

            string dataString  = $"{Name};{AgeStr};{Continent};{Pets};{PreferredTempStr}";
            lblStatus.Content = "Registered Successfully: " + dataString;
            File.AppendAllText("..\\..\\dataString.txt",dataString + "\n");
        }


        private void sliderPerferredTemp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderPerferredTemp.IsLoaded)//need this code to ake sure slider is loaded, before trying to get value from it.
            {
                double PreferredTemp = sliderPerferredTemp.Value;
                lblTemp.Content = Math.Round(PreferredTemp, 0).ToString();
            }

            
        }
    }
}
