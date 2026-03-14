using System;
using System.Collections.Generic;
using System.Data;
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
using PRESENTATION_LAYER.Users;
using PRESENTATION_LAYER.Tasks;
using PRESENTATION_LAYER.TaskTypes;
using BUSINESS_LAYER;

namespace PRESENTATION_LAYER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int TaskTypeID;

        public MainWindow()
        {

            InitializeComponent();

            //Get task types from database using data access layer
            DataTable TaskTypes = clsTaskType.GetTaskTypes(General.User.UserID);
            if (TaskTypes.Rows.Count > 0)
            {
                TaskTypeID = Convert.ToInt32(TaskTypes.Rows[0]["TaskTypeID"]);
            }

        }

        //This function shows tasks and removes the old ones
        private void ShowTasks()
        {

            spTasks.Children.Clear();

            //Getting tasks into a data table
            DataTable Tasks = clsTask.GetTasks(TaskTypeID);

            //Show text says that no task is available
            tbNoTasks.Visibility = (Tasks.Rows.Count == 0) ? Visibility.Visible : Visibility.Collapsed;

            //Add tasks from data table as controls and show them
            foreach (DataRow row in Tasks.Rows)
            {

                clsTask task = clsTask.FindTask(Convert.ToInt32(row["TaskID"]));
                TaskUserControl TaskControl = new TaskUserControl(task, ShowTasks, ShowTasks);

                TaskControl.DeleteFunc += () => ShowTasks();

                spTasks.Children.Add(TaskControl);

            }

        }

        //Show task types in a list so the use can change the current one
        private void FillTaskTypes()
        {

            DataTable TaskTypes = clsTaskType.GetTaskTypes(General.User.UserID);

            //Show text says that no task type is available
            tbNoTaskTypes.Visibility = TaskTypes.Rows.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

            foreach (DataRow TaskType in TaskTypes.Rows)
            {
                lbTaskTypes.Items.Add(TaskType["Name"].ToString());
            }

            if(TaskTypes.Rows.Count != 0)
            {
                lbTaskTypes.SelectedIndex = 0;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTaskTypes();
            ShowTasks();
        }

        //User can choose color for task type to show it in task types list
        private void ColorTaskTypes()
        {

            if (lbTaskTypes.Items == null)
                return;

            for (int i = 0; i < lbTaskTypes.Items.Count; i++)
            {
                ListBoxItem ItemContainer = (ListBoxItem)lbTaskTypes.ItemContainerGenerator.ContainerFromIndex(i);

                if (ItemContainer == null)
                    continue;

                string TaskTypeName = lbTaskTypes.Items[i].ToString();
                clsTaskType TaskType = clsTaskType.FindTaskType(TaskTypeName);

                if (TaskType != null)
                {

                    //If task type has no color it will choose a default color
                    if (TaskType.Color == null || TaskType.Color == string.Empty)
                    {
                        ItemContainer.Background = Brushes.LightGray;
                        ItemContainer.Foreground = Brushes.Black;
                        continue;
                    }

                    ItemContainer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TaskType.Color));
                    SolidColorBrush bgBrush = ItemContainer.Background as SolidColorBrush;

                    if (bgBrush != null)
                    {
                        double luminance = 0.2126 * bgBrush.Color.R + 0.7152 * bgBrush.Color.G + 0.0722 * bgBrush.Color.B;
                        ItemContainer.Foreground = luminance > 128 ? Brushes.Black : Brushes.White;
                    }

                }

            }

        }

        private void ShowTaskTypes()
        {

            lbTaskTypes.Items.Clear();
            FillTaskTypes();
            Dispatcher.BeginInvoke(new Action(() => ColorTaskTypes()), System.Windows.Threading.DispatcherPriority.Loaded);

        }

        //Show the tasks of the choosen task type by user
        private void lbTaskTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbTaskTypes.SelectedIndex >= 0)
            {

                string SelectedTaskType = lbTaskTypes.SelectedItem.ToString();
                clsTaskType TaskType = clsTaskType.FindTaskType(SelectedTaskType);

                if (TaskType != null)
                {

                    TaskTypeID = TaskType.TaskTypeID;
                    ShowTasks();

                }

            }

        }

        private void btnAddTaskType_Click(object sender, RoutedEventArgs e)
        {

            AddEditTaskTypeWindow AddTaskTypeWindow = new AddEditTaskTypeWindow(ShowTaskTypes);
            AddTaskTypeWindow.ShowDialog();

        }

        private void btnEditTaskType_Click(object sender, RoutedEventArgs e)
        {

            AddEditTaskTypeWindow EditTaskTypeWindow = new AddEditTaskTypeWindow(clsTaskType.FindTaskType(TaskTypeID), ShowTaskTypes);
            EditTaskTypeWindow.ShowDialog();

        }

        private void btnDeleteTaskType_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to delete this task type, and all tasks that are related to?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (clsTaskType.GetTaskTypes(General.User.UserID).Rows.Count == 1)
                {
                    MessageBox.Show("You must have at least one task type. Please add a new task type before deleting this one.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (clsTaskType.DeleteTaskType(TaskTypeID))
                {
                    MessageBox.Show("Task Type has been deleted successfully", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowTaskTypes();
                    TaskTypeID = clsTaskType.GetTaskTypes(General.User.UserID).Rows.Count > 0 ? Convert.ToInt32(clsTaskType.GetTaskTypes(General.User.UserID).Rows[0]["TaskTypeID"]) : 0;
                    ShowTasks();
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the task type", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
                MessageBox.Show("Deletion has been cancelled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {

            if (General.User != null)
            {

                AddEditTaskWindow AddEditTask = new AddEditTaskWindow(ShowTasks, TaskTypeID);
                AddEditTask.ShowDialog();

            }

        }
    }

}