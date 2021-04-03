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
using System.Windows.Shapes;

namespace Day011CarOwner
{
    /// <summary>
    /// Interaction logic for ManageCarDlg.xaml
    /// </summary>
    public partial class ManageCarDlg : Window
    {
        CarOwnerDbContext ctx;
        int ownerId;
        public ManageCarDlg(Owner owner)
        {
            InitializeComponent();
            ctx = new CarOwnerDbContext();
            ownerId = owner.Id;
            lvCars.ItemsSource = ctx.Cars.Include("Owner").Where(c =>c.Owner.Id == ownerId).ToList<Car>();
            lblOwner.Content = owner.Name;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbMakeModel.Text.Length>0)
            {
               
                if (ownerId<=0)//
                {
                    MessageBox.Show("Can't find this owner in the database", "owner not found");
                    return;
                }
                Owner curOwner = ctx.Owners.Where(o => o.Id == ownerId).FirstOrDefault<Owner>();
                Car car = new Car { MakeModel = tbMakeModel.Text, Owner = curOwner };
                ctx.Cars.Add(car);
                ctx.SaveChanges();
                lvCars.ItemsSource = ctx.Cars.Include("Owner").Where(c => c.Owner.Id == ownerId).ToList<Car>();
            }
        }
    }
}
