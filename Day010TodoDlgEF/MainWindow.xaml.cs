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

namespace Day010TodoDlgEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TodoDbContext ctx;
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddEditDlg dlg = new AddEditDlg(null);
            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {

                


                try
                {
                    string task = dlg.tbTask.Text;
                    int difficulty = (int)dlg.sliderDifficulty.Value;
                    //DateTime dueDate = (DateTime)dlg.dpDueDate.SelectedDate;//InvalidOperationException
                    
                    DateTime dueDate = (DateTime)dlg.dpDueDate.SelectedDate;
                    EStatus status = (EStatus)Enum.Parse(typeof(EStatus), dlg.comboEStatus.Text, true);

                    Todo todo = new Todo { Task = task, Difficulty = difficulty, DueDate = dueDate, TStatus = status };
                    ctx.Todos.Add(todo);
                    ctx.SaveChanges();
                    lvTodos.ItemsSource = (from t in ctx.Todos orderby t.DueDate select t).ToList<Todo>();
                }
                catch (Exception ex) when (ex is InvalidDataException || ex is InvalidOperationException)
                {

                    MessageBox.Show(this, "Error found on the data:\n" + ex.Message, "Error found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvTodos.SelectedItem == null)
            {
                return;
            }

            Todo selTodo = (Todo)lvTodos.SelectedItem;
            int selId = selTodo.Id;



            AddEditDlg dlg = new AddEditDlg(selTodo);
            dlg.Owner = this;

            if (dlg.ShowDialog() == true)
            {




                Todo upTodo = (from t in ctx.Todos where t.Id == selId select t).FirstOrDefault<Todo>();
                if (upTodo != null)
                {
                    upTodo.Task = dlg.tbTask.Text;
                    upTodo.Difficulty = (int)dlg.sliderDifficulty.Value;
                    upTodo.DueDate = (DateTime)dlg.dpDueDate.SelectedDate;
                    upTodo.TStatus = (EStatus)Enum.Parse(typeof(EStatus), dlg.comboEStatus.Text, true);
                    ctx.SaveChanges();//need this line otherwise no change will apply to the database.
                    lvTodos.ItemsSource = (from t in ctx.Todos orderby t.DueDate select t).ToList<Todo>();
                    tbStatus.Text = $"Record updated: {upTodo.Id}; {upTodo.Task}; {upTodo.Difficulty}; {upTodo.DueDate}; {upTodo.TStatus}";

                }
                else
                {
                    Console.WriteLine("Record to update not found");
                }




            }

        }


        private void lvTodos_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Invoke ContextMenu
            ContextMenu cm = this.FindResource("cmLvTodos") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

       

        private void cmLvTodos_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (lvTodos.SelectedItem == null) return;
            Todo selTodo = (Todo)lvTodos.SelectedItem;
            int selId= selTodo.Id;

            string msg = "Do you want to delete following record?\n" + selTodo.ToString();
            MessageBoxResult result = MessageBox.Show(msg, "Deleting a record", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                Todo upTodo = (from t in ctx.Todos where t.Id == selId select t).FirstOrDefault<Todo>();
                if (upTodo != null)
                {
                    ctx.Todos.Remove(upTodo);
                    ctx.SaveChanges();//need this line otherwise no change will apply to the database.
                    lvTodos.ItemsSource = (from t in ctx.Todos orderby t.DueDate select t).ToList<Todo>();
                    tbStatus.Text = $"Record Delete: {selTodo.Id}; {selTodo.Task}; {selTodo.Difficulty}; {selTodo.DueDate}; {selTodo.TStatus}";

                }
                else
                {
                    Console.WriteLine("Record to update not found");
                }

            }
        }

        private void miExportSelection_Click(object sender, RoutedEventArgs e)
        {

            if (lvTodos.SelectedItem == null) return;
            Todo selTodo = (Todo)lvTodos.SelectedItem;
            int selId = selTodo.Id;

            List<Todo> selTodoList = lvTodos.SelectedItems.Cast<Todo>().ToList();

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
            SaveDataToFile(TargetFileName, selTodoList);
            lvTodos.SelectedItems.Clear();
        }

        private void SaveDataToFile(string fileName, List<Todo> ToBeSavedRecordList)
        {
            try
            {
                List<string> linesList = new List<string>();
                foreach (Todo f in ToBeSavedRecordList)
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

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        public MainWindow()
        {
            InitializeComponent();
            ctx = new TodoDbContext();
            lvTodos.ItemsSource = (from t in ctx.Todos orderby t.DueDate select t).ToList<Todo>();
        }
    }
}
