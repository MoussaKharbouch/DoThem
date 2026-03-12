using System;
using System.Data;
using DATA_LAYER;

namespace BUSINESS_LAYER
{
    /// <summary>
    /// Represents a Task in the system.
    /// Handles business logic related to tasks and communicates
    /// with the Data Layer.
    /// </summary>
    public class clsTask
    {
        /// <summary>
        /// Defines whether the object is in Add or Update mode.
        /// </summary>
        enum enMode { Add, Update }
        enMode Mode;

        /// <summary>
        /// Primary key of the task.
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// Name of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Task status (NotStarted or Completed).
        /// </summary>
        public enum enStatus { NotStarted = 1, Completed = 2 }

        /// <summary>
        /// Current status of the task.
        /// </summary>
        public enStatus Status { get; set; }

        /// <summary>
        /// Priority levels available for a task.
        /// </summary>
        public enum enPriorityLevel { Low = 1, Medium = 2, High = 3 }

        /// <summary>
        /// Current priority level of the task.
        /// </summary>
        public enPriorityLevel PriorityLevel { get; set; }

        /// <summary>
        /// Due date of the task.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Backing field for TaskTypeID.
        /// </summary>
        private int _TaskTypeID;

        /// <summary>
        /// Foreign key referencing the TaskType.
        /// Automatically loads the related TaskType object.
        /// </summary>
        public int TaskTypeID
        {
            get { return _TaskTypeID; }

            set
            {
                clsTaskType TaskType = clsTaskType.FindTaskType(value);

                // If the task type does not exist, ignore assignment
                if (TaskType == null)
                    return;

                _TaskTypeID = value;
                this.TaskType = TaskType;
            }
        }

        /// <summary>
        /// Navigation property for related TaskType.
        /// </summary>
        public clsTaskType TaskType { get; set; }

        /// <summary>
        /// Default constructor used when creating a new Task.
        /// </summary>
        public clsTask()
        {
            TaskID = -1;
            Name = string.Empty;
            Description = string.Empty;

            Status = enStatus.NotStarted;
            PriorityLevel = enPriorityLevel.Medium;

            DueDate = DateTime.MinValue;
            TaskTypeID = -1;

            Mode = enMode.Add;
        }

        /// <summary>
        /// Private constructor used internally when loading
        /// an existing Task from database.
        /// </summary>
        private clsTask(int TaskID, string Name, string Description,
                        short Status, short PriorityLevel,
                        DateTime DueDate, int TaskTypeID)
        {
            this.TaskID = TaskID;
            this.Name = Name;
            this.Description = Description;

            this.Status = (enStatus)Status;
            this.PriorityLevel = (enPriorityLevel)PriorityLevel;

            this.DueDate = DueDate;
            this.TaskTypeID = TaskTypeID;

            Mode = enMode.Update;
        }

        /// <summary>
        /// Finds a Task by its ID.
        /// Returns null if not found.
        /// </summary>
        public static clsTask FindTask(int TaskID)
        {
            string Name = string.Empty;
            string Description = string.Empty;
            short Status = 0;
            short PriorityLevel = -1;
            DateTime DueDate = DateTime.MinValue;
            int TaskTypeID = -1;

            TasksData.FindTask(TaskID, ref Name, ref Description,
                               ref Status, ref PriorityLevel,
                               ref DueDate, ref TaskTypeID);

            if (Name == string.Empty)
                return null;

            return new clsTask(TaskID, Name, Description, Status,
                               PriorityLevel, DueDate, TaskTypeID);
        }

        /// <summary>
        /// Adds new Task to database.
        /// </summary>
        private bool Add()
        {
            int TaskID = this.TaskID;

            bool succeeded = TasksData.AddTask(ref TaskID, Name, Description,
                                               (short)Status, (short)PriorityLevel,
                                               DueDate, TaskTypeID);

            this.TaskID = TaskID;

            return succeeded;
        }

        /// <summary>
        /// Updates existing Task in database.
        /// </summary>
        private bool Update()
        {
            return TasksData.UpdateTask(TaskID, Name, Description,
                                        (short)Status, (short)PriorityLevel,
                                        DueDate, TaskTypeID);
        }

        /// <summary>
        /// Saves current object state (Add or Update automatically).
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
        /// Deletes a Task by ID.
        /// </summary>
        public static bool DeleteTask(int TaskID)
        {
            if (!DoesTaskExist(TaskID))
                return false;

            return TasksData.DeleteTask(TaskID);
        }

        /// <summary>
        /// Checks whether a Task exists.
        /// </summary>
        public static bool DoesTaskExist(int TaskID)
        {
            return TasksData.DoesTaskExist(TaskID);
        }

        /// <summary>
        /// Returns all tasks belonging to a specific TaskType.
        /// </summary>
        static public DataTable GetTasks(int TaskTypeID)
        {
            return TasksData.GetTasks(TaskTypeID);
        }
    }
}