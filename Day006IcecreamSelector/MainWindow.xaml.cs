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

namespace Day006IcecreamSelector
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

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> SelectedItemList = new List<string>();
            if (lbFlavs.SelectedItems == null)
            {
                return;
            }
            foreach (ListBoxItem item in lbFlavs.SelectedItems)
            {
                string value = item.DataContext.ToString();
                SelectedItemList.Add(value);
            }


            //set string values from SelectedItemList to the listbox lbSelection

            for (int i = 0; i < SelectedItemList.Count; i++)
            {
                string SelectedItemStr = SelectedItemList[i].ToString();
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = SelectedItemList[i];
                lbSelections.Items.Add(lbi);
            }


            lbFlavs.SelectedItems.Clear();
            lblStatus.Content = SelectedItemList.Count();
        }

        private void btDeleteScoop_Click(object sender, RoutedEventArgs e)
        {
            int CountSelectedItems = lbSelections.SelectedItems.Count;
            if (CountSelectedItems == 0)
            {
                return;
            }


            //the lbSelections.SelectedItems will change once we start to removing items, need to store the items to a list as to-be-removed target
            List<ListBoxItem> selectedItemList = new List<ListBoxItem>();
            for (int i = 0; i < CountSelectedItems; i++)
            {
                ListBoxItem lbi = (ListBoxItem)lbSelections.SelectedItems[i];
                selectedItemList.Add(lbi);
            }


            //need to use selectedItemList.Count, not lbSelections.SelectedItems.Count, as the it will deduct as removing items, and cause out of bound exception
            for (int i = 0; i < selectedItemList.Count; i++)
            {
                ListBoxItem lbi = selectedItemList[i];
                lbSelections.Items.Remove(lbi);
            }


        }

        private void btClearAll_Click(object sender, RoutedEventArgs e)
        {

            lbSelections.Items.Clear();
        }
    }
}
