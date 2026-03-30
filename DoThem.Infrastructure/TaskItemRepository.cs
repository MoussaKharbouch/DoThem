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
        throw new NotImplementedException();
    }

    public bool DoesTaskExist(int taskID)
    {
        throw new NotImplementedException();
    }

    public bool DoesTaskExist(string taskName, int userID, int taskTypeID)
    {
        throw new NotImplementedException();
    }

    public int? AddTask(TaskItem task)
    {
        throw new NotImplementedException();
    }

    public bool UpdateTask(int taskID, TaskItem newTask)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTask(int taskID)
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks()
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks(int userID)
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks(int userID, int taskTypeID)
    {
        throw new NotImplementedException();
    }

}
