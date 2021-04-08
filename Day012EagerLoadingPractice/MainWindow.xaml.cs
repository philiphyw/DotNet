using Day012EagerLoadingPractice.DataLayer;
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

namespace Day012EagerLoadingPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       CarsOwnersDbContext ctx = new DataLayer.CarsOwnersDbContext();
        public MainWindow()
        {
            InitializeComponent();
            lvOwners.ItemsSource = ctx.Owners.OrderBy(o => o.Id).Cast<Owner>().ToList();
            lvCars.ItemsSource = ctx.Cars.OrderBy(o => o.Id).Cast<Car>().ToList();
        }

        private void btAddOwner_Click(object sender, RoutedEventArgs e)
        {
            Owner owner = new Owner { Name = tbOwnerName.Text };
            ctx.Owners.Add(owner);
            lvOwners.ItemsSource = ctx.Owners.OrderBy(o => o.Id).Cast<Owner>().ToList();
        }

        private void btAddCar_Click(object sender, RoutedEventArgs e)
        {
            Car car = new Car { MakeModel = tbCarMakeModel.Text };
            ctx.Cars.Add(car);
            lvCars.ItemsSource = ctx.Cars.OrderBy(o => o.Id).Cast<Car>().ToList();
        }
    }
}
