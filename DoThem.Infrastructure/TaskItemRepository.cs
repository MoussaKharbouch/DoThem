using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public class TaskItemRepository : ITaskItemRepository
{

    /// <summary>
    /// the connection string is private to keep it safe,
    /// it is like the password of database
    /// </summary>
    private string _ConnectionString = string.Empty;

    /// <summary>
    /// a constructor that takes the connection screen from presentation layer
    /// </summary>
    public TaskItemRepository(string connectionString)
    {
        this._ConnectionString = connectionString;
    }

    public TaskItem? FindTask(int taskID)
    {
        
        // query to retrieve data using sql statement with user id
        string query = @"SELECT * FROM Tasks
                        Where TaskID = @TaskID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the user id parameter
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TaskID", taskID);

                    // use reader to get data from database
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        /// if reader doesn't have any rows,
                        /// we return null
                        if (!reader.HasRows)
                            return null;

                        if (reader.Read())
                        {

                            /// make sure that the title is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string title = reader["Title"]?.ToString() ?? throw new Exception("Title null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            // due date can be null, so we use null-coalescing to return DateTime.MaxValue if it is null, which means that the task has no due date
                            DateTime? dueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : null;

                            // make sure that the task status is in the right range
                            TaskItem.TaskStatus status = Enum.IsDefined(typeof(TaskItem.TaskStatus), Convert.ToInt32(reader["Status"])) ? (TaskItem.TaskStatus)Convert.ToInt32(reader["Status"]) : TaskItem.TaskStatus.NotStarted;

                            return new TaskItem(
                                TaskID: Convert.ToInt32(reader["TaskID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Title: title,
                                Description: description,
                                TaskTypeId: Convert.ToInt32(reader["TaskTypeID"]),
                                CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                                DueDate: dueDate,
                                Status: status
                            );

                        }

                    }

                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Finding task failed.", ex);
        }

        return null;

    }

    public TaskItem? FindTask(string taskName, int userID, int taskTypeID)
    {

        // query to retrieve data using sql statement with task name, user id, and task type id
        string query = @"SELECT * FROM Tasks
                        Where Title = @Title AND UserID = @UserID AND TaskTypeID = @TaskTypeID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the parameters
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Title", taskName);
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

                    // use reader to get data from database
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        /// if reader doesn't have any rows,
                        /// we return null
                        if (!reader.HasRows)
                            return null;

                        if (reader.Read())
                        {

                            /// make sure that the title is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string title = reader["Title"]?.ToString() ?? throw new Exception("Title null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            // due date can be null
                            DateTime? dueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : null;

                            // make sure that the task status is in the right range
                            TaskItem.TaskStatus status = Enum.IsDefined(typeof(TaskItem.TaskStatus), Convert.ToInt32(reader["Status"])) ? (TaskItem.TaskStatus)Convert.ToInt32(reader["Status"]) : TaskItem.TaskStatus.NotStarted;

                            return new TaskItem(
                                TaskID: Convert.ToInt32(reader["TaskID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Title: title,
                                Description: description,
                                TaskTypeId: Convert.ToInt32(reader["TaskTypeID"]),
                                CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                                DueDate: dueDate,
                                Status: status
                            );

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Finding task failed.", ex);
        }

        return null;

    }

    public bool DoesTaskExist(int taskID)
    {

        // query to retrieve data using sql statement with task id
        string query = @"SELECT 1 FROM Tasks
                        Where TaskID = @TaskID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the task id parameter
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TaskID", taskID);

                    // use scalar to retrieve single value from database
                    object result = command.ExecuteScalar();
                    return (result != null && result != DBNull.Value);

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Checking if task exists failed.", ex);
        }

    }

    public bool DoesTaskExist(string taskName, int userID, int taskTypeID)
    {

        // query to retrieve data using sql statement with task name, user id, and task type id
        string query = @"SELECT 1 FROM Tasks
                        Where Title = @Title AND UserID = @UserID AND TaskTypeID = @TaskTypeID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the parameters
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Title", taskName);
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

                    // use scalar to retrieve single value from database
                    object result = command.ExecuteScalar();
                    return (result != null && result != DBNull.Value);

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Checking if task exists failed.", ex);
        }

    }

    public int? AddTask(TaskItem task)
    {

        // the query that adds a new task to database (at the end, we get the id of the new task)
        string query = @"INSERT INTO [dbo].[Tasks]
                                ([Title]
                                ,[Description]
                                ,[UserID]
                                ,[TaskTypeID]
                                ,[CreationDate]
                                ,[DueDate]
                                ,[Status])
                            VALUES
                                (@Title
                                ,@Description
                                ,@UserID
                                ,@TaskTypeID
                                ,@CreationDate
                                ,@DueDate
                                ,@Status);
                        SELECT SCOPE_IDENTITY();";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to add new task (we add all parameters from the object)
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameters
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@UserID", task.UserID);
                    command.Parameters.AddWithValue("@TaskTypeID", task.TaskTypeId);
                    command.Parameters.AddWithValue("@CreationDate", task.CreationDate);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", task.Status);

                    // we retrieve the id of the added task
                    object result = command.ExecuteScalar();
                    int newTaskID;

                    // checking if the value is valid (it can return nothing)
                    if (result != null && int.TryParse(result.ToString(), out newTaskID))
                        return newTaskID;
                    else
                        return null;

                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Adding new task failed", ex);
        }

    }

    public bool UpdateTask(int taskID, TaskItem newTask)
    {

        // the query that updates a task in database
        string query = @"UPDATE [dbo].[Tasks]
                                SET [Title] = @Title
                                    ,[Description] = @Description
                                    ,[UserID] = @UserID
                                    ,[TaskTypeID] = @TaskTypeID
                                    ,[CreationDate] = @CreationDate
                                    ,[DueDate] = @DueDate
                                    ,[Status] = @Status
                                WHERE [TaskID] = @TaskID";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to update task (we add all parameters from the object)
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameters
                    command.Parameters.AddWithValue("@Title", newTask.Title);
                    command.Parameters.AddWithValue("@Description", newTask.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@UserID", newTask.UserID);
                    command.Parameters.AddWithValue("@TaskTypeID", newTask.TaskTypeId);
                    command.Parameters.AddWithValue("@CreationDate", newTask.CreationDate);
                    command.Parameters.AddWithValue("@DueDate", newTask.DueDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", newTask.Status);
                    command.Parameters.AddWithValue("@TaskID", taskID);

                    // checking if the update was successful (it can return nothing)
                    if (command.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;

                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Updating task failed", ex);
        }

    }

    public bool DeleteTask(int taskID)
    {

        // the query that deletes a task from database
        string query = @"DELETE FROM [dbo].[Tasks]
                        WHERE [TaskID] = @TaskID";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to delete task by id
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameter
                    command.Parameters.AddWithValue("@TaskID", taskID);

                    // checking if the delete was successful (it can return nothing)
                    if (command.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;

                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Deleting task failed", ex);
        }

    }

    public List<TaskItem> GetTasks()
    {

        List<TaskItem> Tasks = new List<TaskItem>();

        // query to retrieve all tasks
        string query = @"SELECT * FROM Tasks";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query
                SqlCommand command = new SqlCommand(query, connection);

                // use reader to get data from database
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    /// if reader doesn't have any rows,
                    /// we return the list directly
                    if (!reader.HasRows)
                        return Tasks;

                    while (reader.Read())
                    {

                        /// make sure that the title is not null,
                        /// and if it is we return an exception, 
                        /// using null-coalescing
                        string title = reader["Title"]?.ToString() ?? throw new Exception("Title null");

                        // the description can be null, so we use null-coalescing to return empty string if it is null
                        string description = reader["Description"]?.ToString() ?? string.Empty;

                        // due date can be null
                        DateTime? dueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : null;

                        // make sure that the task status is in the right range
                        TaskItem.TaskStatus status = Enum.IsDefined(typeof(TaskItem.TaskStatus), Convert.ToInt32(reader["Status"])) ? (TaskItem.TaskStatus)Convert.ToInt32(reader["Status"]) : TaskItem.TaskStatus.NotStarted;

                        // Defining task from current row in database
                        TaskItem task = new TaskItem(
                            TaskID: Convert.ToInt32(reader["TaskID"]),
                            UserID: Convert.ToInt32(reader["UserID"]),
                            Title: title,
                            Description: description,
                            TaskTypeId: Convert.ToInt32(reader["TaskTypeID"]),
                            CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                            DueDate: dueDate,
                            Status: status
                        );

                        Tasks.Add(task);

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting tasks failed.", ex);
        }

        return Tasks;

    }

    public List<TaskItem> GetTasks(int userID)
    {

        List<TaskItem> Tasks = new List<TaskItem>();

        // query to retrieve tasks by user id
        string query = @"SELECT * FROM Tasks
                        Where UserID = @UserID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the user id parameter
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@UserID", userID);

                    // use reader to get data from database
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        /// if reader doesn't have any rows,
                        /// we return the list directly
                        if (!reader.HasRows)
                            return Tasks;

                        while (reader.Read())
                        {

                            /// make sure that the title is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string title = reader["Title"]?.ToString() ?? throw new Exception("Title null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            // due date can be null
                            DateTime? dueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : null;

                            // make sure that the task status is in the right range
                            TaskItem.TaskStatus status = Enum.IsDefined(typeof(TaskItem.TaskStatus), Convert.ToInt32(reader["Status"])) ? (TaskItem.TaskStatus)Convert.ToInt32(reader["Status"]) : TaskItem.TaskStatus.NotStarted;

                            // Defining task from current row in database
                            TaskItem task = new TaskItem(
                                TaskID: Convert.ToInt32(reader["TaskID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Title: title,
                                Description: description,
                                TaskTypeId: Convert.ToInt32(reader["TaskTypeID"]),
                                CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                                DueDate: dueDate,
                                Status: status
                            );

                            Tasks.Add(task);

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting tasks failed.", ex);
        }

        return Tasks;

    }

    public List<TaskItem> GetTasks(int userID, int taskTypeID)
    {

        List<TaskItem> Tasks = new List<TaskItem>();

        // query to retrieve tasks by user id and task type id
        string query = @"SELECT * FROM Tasks
                        Where UserID = @UserID AND TaskTypeID = @TaskTypeID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the parameters
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

                    // use reader to get data from database
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        /// if reader doesn't have any rows,
                        /// we return the list directly
                        if (!reader.HasRows)
                            return Tasks;

                        while (reader.Read())
                        {

                            /// make sure that the title is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string title = reader["Title"]?.ToString() ?? throw new Exception("Title null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            // due date can be null
                            DateTime? dueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : null;

                            // make sure that the task status is in the right range
                            TaskItem.TaskStatus status = Enum.IsDefined(typeof(TaskItem.TaskStatus), Convert.ToInt32(reader["Status"])) ? (TaskItem.TaskStatus)Convert.ToInt32(reader["Status"]) : TaskItem.TaskStatus.NotStarted;

                            // Defining task from current row in database
                            TaskItem task = new TaskItem(
                                TaskID: Convert.ToInt32(reader["TaskID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Title: title,
                                Description: description,
                                TaskTypeId: Convert.ToInt32(reader["TaskTypeID"]),
                                CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                                DueDate: dueDate,
                                Status: status
                            );

                            Tasks.Add(task);

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting tasks failed.", ex);
        }

        return Tasks;

    }

}
