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

namespace Day008Todo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Todo> todoList;
        private const string DATA_SOURCE = "..\\..\\todos.txt";
        private bool _isModified = false;

        private void LoadDataFromFile()
        {

            try
            {
                List<string> dataStringList = File.ReadAllLines(DATA_SOURCE).ToList();
                if (dataStringList.Count == 0)
                {
                    return;
                }

                foreach (string dataString in dataStringList)
                {
                    todoList.Add(new Todo(dataString));
                }
                RefreshTodoList();
                lblStatus.Content = todoList.Count();
            }
            catch (Exception ex) when (ex is InvalidDataException || ex is IOException)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error found");
            }


        }

        private void SaveDataToFile(string FileName)
        {
            List<string> DataStringList = new List<string>();
            string TargetFileName = FileName;

            if (TargetFileName == null || TargetFileName.Length==0)
            {
                if (lvTodoList.SelectedItem == null)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Please select a record to save", "Confirmation");
                    return;
                }

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.TXT";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.AddExtension = true;


                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName == "")
                {
                    lblStatus.Content = "Cancel saving";
                    return;
                }

                TargetFileName = saveFileDialog.FileName;

                foreach (Todo item in lvTodoList.SelectedItems)
                {
                    string DataString = item.ToDataString();
                    DataStringList.Add(DataString);
                }
            }
            else
            {
                DataStringList = todoList.Select(t => t.ToDataString()).ToList();
            }
           



            File.WriteAllLines(TargetFileName, DataStringList);

            System.Windows.MessageBox.Show($"Saved data to {TargetFileName}", "Data Saved");
            lblStatus.Content = $"{DataStringList.Count} record(s) saved";

            lvTodoList.SelectedItems.Clear();


        }




        //Event Hanlders
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isModified)
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
                    SaveDataToFile(DATA_SOURCE);
                }
            }
        }

        private void ButtonExportToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToFile(null);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string task = tbTask.Text;
                int difficulty = (int)sliderDifficulty.Value;            
                DateTime dueDate = dpDueDate.SelectedDate.Value;
                Status status = (Status)cbStatus.SelectedItem;

                Todo todo = new Todo($"{task};{difficulty};{dueDate:d};{status}");
                todoList.Add(todo);
                RefreshTodoList();
                _isModified = true;
                lblStatus.Content = "Added new record";
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is InvalidDataException)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }


        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvTodoList.SelectedItems.Count==0)
            {
                lblStatus.Content = "No records are selected";
                return;
            }

            //build a intermediate list to store to-be-removed item list, since the content of lvTodoList.SelectedItems will change once start to remove item
            List<Todo> selectedItemList = lvTodoList.SelectedItems.Cast<Todo>().ToList();


            //Need to remove the item from the intemsource todoList, can NOT remove from the lvTodoList, otherwise will thorugh InvalidOperationException
            lvTodoList.SelectedItems.Clear();
            foreach (Todo t in selectedItemList)
            {
               todoList.Remove(t);

                
            }

            //Clear the values of deleted item from Input Fields
            tbTask.Text = "";
            sliderDifficulty.Value = 1;
            dpDueDate.SelectedDate = null;
            cbStatus.SelectedItem = null;

            _isModified = true;
            RefreshTodoList();
            lblStatus.Content = $"{selectedItemList.Count} record(s) removed.";

        }

        private void RefreshTodoList()
        {
            todoList.Sort((t1, t2) => t1.DueDate.CompareTo(t2.DueDate));
            lvTodoList.Items.Refresh();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvTodoList.SelectedItems.Count == 0)
            {
                lblStatus.Content = "No records are selected";
                return;
                
            }

            try
            {
                string task = tbTask.Text;
                int difficulty = (int)sliderDifficulty.Value;
                DateTime dueDate = dpDueDate.SelectedDate.Value;
                Status status = (Status)cbStatus.SelectedItem;

                //update first/default selected item with the input fields
                int selectedIndex = lvTodoList.SelectedIndex;
               
                Todo todo = new Todo($"{task};{difficulty};{dueDate:d};{status}");
                todoList[selectedIndex] = todo;
                lvTodoList.Items.Refresh();//inform the lbTripList has been changed
                _isModified = true;
                lblStatus.Content = "Updated record";
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is InvalidDataException)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            todoList = new List<Todo>();
            lvTodoList.ItemsSource = todoList;
            LoadDataFromFile();

            //set combo box itemsource from the enum class Status
            cbStatus.ItemsSource = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
         
        }

        private void lvTodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = lvTodoList.SelectedIndex;

            if (selectedIndex<0)
            {
                return;
            }

            Todo selectedTodo = todoList[selectedIndex];

            tbTask.Text = selectedTodo.Task;
            sliderDifficulty.Value = selectedTodo.Difficulty;
            dpDueDate.Text = $"{selectedTodo.DueDate:d}";
           
            cbStatus.Text = selectedTodo.Status.ToString();

        }



        private void lvTodoList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Invoke ContextMenu
            ContextMenu cm = this.FindResource("cmLvTodoList") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }


        private void cmLvTodoList_DeleteClick(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var toDeleteFromBindedList = (Todo)item.SelectedCells[0].Item;

            //Remove the toDeleteFromBindedList object from your ObservableCollection
            lvTodoList.Items.Remove(toDeleteFromBindedList);
        }
        private void cmLvTodoList_UpdateClick(object sender, RoutedEventArgs e)
        {
            //Invoke ContextMenu
            ContextMenu cm = this.FindResource("cmLvTodoList") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

    }
}
