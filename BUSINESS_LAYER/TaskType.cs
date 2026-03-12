using System;
using System.Data;
using DATA_LAYER;

namespace BUSINESS_LAYER
{
    /// <summary>
    /// Represents a Task Type in the system.
    /// Responsible for managing task type business logic and
    /// communicating with the Data Layer.
    /// </summary>
    public class clsTaskType
    {
        /// <summary>
        /// Defines the current operation mode (Add or Update).
        /// </summary>
        enum enMode { Add, Update }
        enMode Mode;

        /// <summary>
        /// Primary Key of TaskType in database.
        /// </summary>
        public int TaskTypeID { get; set; }

        /// <summary>
        /// Name of the task type (e.g., Work, Study, Personal).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date when the task type was created.
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Color assigned to this task type (used in UI).
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Description of the task type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Backing field for UserID property.
        /// </summary>
        private int _UserID;

        /// <summary>
        /// Foreign key referencing the User who owns this TaskType.
        /// Automatically loads the related User object.
        /// </summary>
        public int UserID
        {
            get { return _UserID; }

            set
            {
                // Load user from database
                User User = User.FindUser(value);

                // If user does not exist, do not assign
                if (User == null)
                    return;

                _UserID = value;
                this.User = User;
            }
        }

        /// <summary>
        /// Navigation property for related User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Default constructor used when creating a new TaskType.
        /// </summary>
        public clsTaskType()
        {
            TaskTypeID = -1;
            Name = string.Empty;
            DateOfCreation = DateTime.MinValue;
            Color = string.Empty;
            Description = string.Empty;
            UserID = -1;
            User = new User();

            Mode = enMode.Add;
        }

        /// <summary>
        /// Private constructor used internally when loading
        /// an existing TaskType from the database.
        /// </summary>
        private clsTaskType(int TaskTypeID, string Name, DateTime DateOfCreation,
                            string Color, string Description, int UserID)
        {
            this.TaskTypeID = TaskTypeID;
            this.Name = Name;
            this.DateOfCreation = DateOfCreation;
            this.Color = Color;
            this.Description = Description;
            this.UserID = UserID;
            this.User = User.FindUser(UserID);

            Mode = enMode.Update;
        }

        /// <summary>
        /// Finds a TaskType by its ID.
        /// Returns null if not found.
        /// </summary>
        public static clsTaskType FindTaskType(int TaskTypeID)
        {
            string Name = string.Empty;
            DateTime DateOfCreation = DateTime.MinValue;
            string Color = string.Empty;
            string Description = string.Empty;
            int UserID = -1;

            TaskTypesData.FindTaskType(TaskTypeID, ref Name, ref DateOfCreation, ref Color, ref Description, ref UserID);

            if (Name == string.Empty)
                return null;

            return new clsTaskType(TaskTypeID, Name, DateOfCreation, Color, Description, UserID);
        }

        /// <summary>
        /// Finds a TaskType by its Name.
        /// Returns null if not found.
        /// </summary>
        public static clsTaskType FindTaskType(string Name)
        {
            int TaskTypeID = -1;
            DateTime DateOfCreation = DateTime.MinValue;
            string Color = string.Empty;
            string Description = string.Empty;
            int UserID = -1;

            TaskTypesData.FindTaskType(Name, ref TaskTypeID, ref DateOfCreation, ref Color, ref Description, ref UserID);

            if (TaskTypeID == -1)
                return null;

            return new clsTaskType(TaskTypeID, Name, DateOfCreation, Color, Description, UserID);
        }

        /// <summary>
        /// Adds new TaskType to database.
        /// </summary>
        private bool Add()
        {
            int TaskTypeID = this.TaskTypeID;

            bool succeeded = TaskTypesData.AddTaskType(ref TaskTypeID, Name, DateOfCreation, Color, Description, UserID);

            this.TaskTypeID = TaskTypeID;

            return succeeded;
        }

        /// <summary>
        /// Updates existing TaskType in database.
        /// </summary>
        private bool Update()
        {
            return TaskTypesData.UpdateTaskType(TaskTypeID, Name, DateOfCreation, Color, Description, UserID);
        }

        /// <summary>
        /// Saves current object.
        /// Automatically decides between Add and Update.
        /// </summary>
        public bool Save()
        {
            bool succeeded = false;

            switch (Mode)
            {
                case enMode.Add:
                    succeeded = Add();

                    if (succeeded)
                        Mode = enMode.Update;
                    break;

                case enMode.Update:
                    succeeded = Update();
                    break;
            }

            return succeeded;
        }

        /// <summary>
        /// Deletes a TaskType by ID.
        /// </summary>
        public static bool DeleteTaskType(int TaskTypeID)
        {
            if (!DoesTaskTypeExist(TaskTypeID))
                return false;

            return TaskTypesData.DeleteTaskType(TaskTypeID);
        }

        /// <summary>
        /// Checks if a TaskType exists in database.
        /// </summary>
        public static bool DoesTaskTypeExist(int TaskTypeID)
        {
            return TaskTypesData.DoesTaskTypeExist(TaskTypeID);
        }

        /// <summary>
        /// Returns all TaskTypes for a specific user.
        /// If the user has no task types, a default one named "General" is created.
        /// </summary>
        static public DataTable GetTaskTypes(int UserID)
        {
            DataTable dt = TaskTypesData.GetTaskTypes(UserID);

            // If user has no task types, create a default one
            if (dt.Rows.Count == 0)
            {
                clsTaskType defaultType = new clsTaskType();
                defaultType.Name = "General";
                defaultType.DateOfCreation = DateTime.Now;
                defaultType.Color = "#808080"; // Default gray color
                defaultType.Description = "Default task type";
                defaultType.UserID = UserID;

                defaultType.Save();

                // Reload task types after creation
                dt = TaskTypesData.GetTaskTypes(UserID);
            }

            return dt;
        }
    }
}