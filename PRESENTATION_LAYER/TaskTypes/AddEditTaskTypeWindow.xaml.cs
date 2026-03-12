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
using BUSINESS_LAYER;

namespace PRESENTATION_LAYER.TaskTypes
{

    /// <summary>
    /// Interaction logic for AddEditTaskTypeWindow.xaml
    /// </summary>
    public partial class AddEditTaskTypeWindow : Window
    {

        private clsTaskType TaskType;
        private Action Save_Event;

        enum enMode { Add, Edit }
        enMode Mode;

        //Constructor for adding new task type
        public AddEditTaskTypeWindow(Action Save_Event)
        {
            InitializeComponent();
            this.TaskType = new clsTaskType();
            this.Save_Event = Save_Event;
            Mode = enMode.Add;
            tbDateOfCreation.Text = DateTime.Now.ToShortDateString();
        }

        //Constructor for editing task type
        public AddEditTaskTypeWindow(clsTaskType TaskType, Action Save_Event)
        {
            InitializeComponent();
            this.TaskType = TaskType;
            this.Save_Event = Save_Event;
            Mode = enMode.Edit;
            ShowData(TaskType);
        }

        //Show task type info
        private void ShowData(clsTaskType TaskType)
        {

            if (TaskType == null)
                return;

            tbName.Text = TaskType.Name;
            tbDescription.Text = TaskType.Description;
            tbColor.Text = TaskType.Color;
            tbDateOfCreation.Text = TaskType.DateOfCreation.ToShortDateString();

        }

        //Check if user has choosen a valid color
        private bool IsValidColor(string colorCode)
        {

            if (string.IsNullOrWhiteSpace(colorCode))
                return true;

            try
            {
                ColorConverter.ConvertFromString(colorCode);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private bool ValidateInput()
        {

            string colorText = tbColor.Text;
            bool colorValid = IsValidColor(colorText);

            if (Mode == enMode.Edit && tbName.Text == TaskType.Name)
                return colorValid;

            //If task type is new then check if its name is unique
            bool nameNotEmpty = tbName.Text != string.Empty;
            bool nameUnique = clsTaskType.FindTaskType(tbName.Text) == null;

            return nameNotEmpty && nameUnique && colorValid;

        }



        private bool Save()
        {

            clsTaskType TaskType = enMode.Add == Mode ? new clsTaskType() : clsTaskType.FindTaskType(this.TaskType.TaskTypeID);

            TaskType.Name = tbName.Text;
            TaskType.Description = tbDescription.Text;
            TaskType.Color = tbColor.Text;
            TaskType.UserID = General.User.UserID;

            if (Mode == enMode.Add)
                TaskType.DateOfCreation = DateTime.Now;

            TaskType.Save();

            return true;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateInput())
            {
                MessageBox.Show("Invalid input. Please check your entries.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Save())
            {
                MessageBox.Show("Error saving task type. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Save_Event?.Invoke();
                this.Close();
            }

        }

    }

}