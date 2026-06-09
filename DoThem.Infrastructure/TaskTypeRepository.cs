using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using DoThem.Domain;
using System.Data;

namespace DoThem.Infrastructure;

public class TaskTypeRepository : ITaskTypeRepository
{

    /// <summary>
    /// the connection string is private to keep it safe,
    /// it is like the password of database
    /// </summary>
    private string _ConnectionString = string.Empty;

    /// <summary>
    /// a constructor that takes the connection string from presentation layer
    /// </summary>
    public TaskTypeRepository(string connectionString)
    {
        this._ConnectionString = connectionString;
    }

    public TaskType? FindTaskType(int taskTypeID)
    {

        // query to retrieve data using sql statement with task type id
        string query = @"SELECT * FROM TaskTypes
                        Where TaskTypeID = @TaskTypeID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the task type id parameter
                using (SqlCommand command = new SqlCommand(query, connection))
                {

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

                            /// make sure that the name is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string name = reader["Name"]?.ToString() ?? throw new Exception("Name null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            return new TaskType(
                                TaskTypeID: Convert.ToInt32(reader["TaskTypeID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Name: name,
                                Description: description,
                                CreationDate: Convert.ToDateTime(reader["CreationDate"])
                            );

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Finding task type failed.", ex);
        }

        return null;

    }

    public TaskType? FindTaskType(string name, int userID)
    {

        // query to retrieve data using sql statement with name and user id
        string query = @"SELECT * FROM TaskTypes
                        Where Name = @Name AND UserID = @UserID";

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

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@UserID", userID);

                    // use reader to get data from database
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        /// if reader doesn't have any rows,
                        /// we return null
                        if (!reader.HasRows)
                            return null;

                        if (reader.Read())
                        {

                            /// make sure that the name is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string taskTypeName = reader["Name"]?.ToString() ?? throw new Exception("Name null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            return new TaskType(
                                TaskTypeID: Convert.ToInt32(reader["TaskTypeID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Name: taskTypeName,
                                Description: description,
                                CreationDate: Convert.ToDateTime(reader["CreationDate"])
                            );

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Finding task type failed.", ex);
        }

        return null;

    }

    public bool DoesTaskTypeExist(int taskTypeID)
    {

        // query to retrieve data using sql statement with task type id
        string query = @"SELECT 1 FROM TaskTypes
                        Where TaskTypeID = @TaskTypeID";

        try
        {

            /// connect to database
            /// we have used "using" in every database operation
            /// for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // open the connection to database
                connection.Open();

                // the command that executes the query using the task type id parameter
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

                    // use scalar to retrieve single value from database
                    object result = command.ExecuteScalar();
                    return (result != null && result != DBNull.Value);

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Checking if task type exists failed.", ex);
        }

    }

    public bool DoesTaskTypeExist(string name, int userID)
    {

        // query to retrieve data using sql statement with name and user id
        string query = @"SELECT 1 FROM TaskTypes
                        Where Name = @Name AND UserID = @UserID";

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

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@UserID", userID);

                    // use scalar to retrieve single value from database
                    object result = command.ExecuteScalar();
                    return (result != null && result != DBNull.Value);

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Checking if task type exists failed.", ex);
        }

    }

    public int? AddTaskType(TaskType taskType)
    {

        // the query that adds a new task type to database (at the end, we get the id of the new task type)
        string query = @"INSERT INTO [dbo].[TaskTypes]
                                ([Name]
                                ,[Description]
                                ,[UserID]
                                ,[CreationDate])
                            VALUES
                                (@Name
                                ,@Description
                                ,@UserID
                                ,@CreationDate);
                        SELECT SCOPE_IDENTITY();";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to add new task type (we add all parameters from the object)
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameters
                    command.Parameters.AddWithValue("@Name", taskType.Name);
                    command.Parameters.AddWithValue("@Description", taskType.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@UserID", taskType.UserID);
                    command.Parameters.AddWithValue("@CreationDate", taskType.CreationDate);

                    // we retrieve the id of the added task type
                    object result = command.ExecuteScalar();
                    int newTaskTypeID;

                    // checking if the value is valid (it can return nothing)
                    if (result != null && int.TryParse(result.ToString(), out newTaskTypeID))
                        return newTaskTypeID;
                    else
                        return null;

                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Adding new task type failed", ex);
        }

    }

    public bool UpdateTaskType(int taskTypeID, TaskType newTaskType)
    {

        // the query that updates a task type in database
        string query = @"UPDATE [dbo].[TaskTypes]
                                SET [Name] = @Name
                                    ,[Description] = @Description
                                    ,[UserID] = @UserID
                                    ,[CreationDate] = @CreationDate
                                WHERE [TaskTypeID] = @TaskTypeID";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to update task type (we add all parameters from the object)
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameters
                    command.Parameters.AddWithValue("@Name", newTaskType.Name);
                    command.Parameters.AddWithValue("@Description", newTaskType.Description ?? string.Empty);
                    command.Parameters.AddWithValue("@UserID", newTaskType.UserID);
                    command.Parameters.AddWithValue("@CreationDate", newTaskType.CreationDate);
                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

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
            throw new Exception("Updating task type failed", ex);
        }

    }

    public bool DeleteTaskType(int taskTypeID)
    {

        // the query that deletes a task type from database
        string query = @"DELETE FROM [dbo].[TaskTypes]
                        WHERE [TaskTypeID] = @TaskTypeID";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                connection.Open();

                // command to delete task type by id
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // add the parameter
                    command.Parameters.AddWithValue("@TaskTypeID", taskTypeID);

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
            throw new Exception("Deleting task type failed", ex);
        }

    }

    public List<TaskType> GetTaskTypes()
    {

        List<TaskType> TaskTypes = new List<TaskType>();

        // query to retrieve all task types
        string query = @"SELECT * FROM TaskTypes";

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
                        return TaskTypes;

                    while (reader.Read())
                    {

                        /// make sure that the name is not null,
                        /// and if it is we return an exception, 
                        /// using null-coalescing
                        string name = reader["Name"]?.ToString() ?? throw new Exception("Name null");

                        // the description can be null, so we use null-coalescing to return empty string if it is null
                        string description = reader["Description"]?.ToString() ?? string.Empty;

                        // Defining task type from current row in database
                        TaskType taskType = new TaskType(
                            TaskTypeID: Convert.ToInt32(reader["TaskTypeID"]),
                            UserID: Convert.ToInt32(reader["UserID"]),
                            Name: name,
                            Description: description,
                            CreationDate: Convert.ToDateTime(reader["CreationDate"])
                        );

                        TaskTypes.Add(taskType);

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting task types failed.", ex);
        }

        return TaskTypes;

    }

    public List<TaskType> GetTaskTypes(int userID)
    {

        List<TaskType> TaskTypes = new List<TaskType>();

        // query to retrieve task types by user id
        string query = @"SELECT * FROM TaskTypes
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
                            return TaskTypes;

                        while (reader.Read())
                        {

                            /// make sure that the name is not null,
                            /// and if it is we return an exception, 
                            /// using null-coalescing
                            string name = reader["Name"]?.ToString() ?? throw new Exception("Name null");

                            // the description can be null, so we use null-coalescing to return empty string if it is null
                            string description = reader["Description"]?.ToString() ?? string.Empty;

                            // Defining task type from current row in database
                            TaskType taskType = new TaskType(
                                TaskTypeID: Convert.ToInt32(reader["TaskTypeID"]),
                                UserID: Convert.ToInt32(reader["UserID"]),
                                Name: name,
                                Description: description,
                                CreationDate: Convert.ToDateTime(reader["CreationDate"])
                            );

                            TaskTypes.Add(taskType);

                        }

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting task types failed.", ex);
        }

        return TaskTypes;

    }

}
