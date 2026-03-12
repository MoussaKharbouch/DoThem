using System;
using System.Data;
using System.Data.SqlClient;

namespace DATA_LAYER
{
    /// <summary>
    /// Provides data access methods for managing Tasks in the database.
    /// Handles CRUD operations and retrieval for Task entities.
    /// </summary>
    public class TasksData
    {
        /// <summary>
        /// Retrieves a task by its ID, populating all properties if found.
        /// </summary>
        public static void FindTask(int TaskID, ref string Name, ref string Description,
                                    ref short Status, ref short PriorityLevel,
                                    ref DateTime DueDate, ref int TaskTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Tasks WHERE TaskID = @TaskID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", TaskID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Name = reader["Name"].ToString();
                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                    Status = short.Parse(reader["Status"].ToString());
                    PriorityLevel = reader["PriorityLevel"] != DBNull.Value ? short.Parse(reader["PriorityLevel"].ToString()) : (short)-1;
                    DueDate = reader["DueDate"] != DBNull.Value ? (DateTime)reader["DueDate"] : DateTime.MinValue;
                    TaskTypeID = int.Parse(reader["TaskTypeID"].ToString());
                    reader.Close();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Checks whether a task exists by TaskID.
        /// Returns true if a record is found.
        /// </summary>
        public static bool DoesTaskExist(int TaskID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM Tasks WHERE TaskID = @TaskID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", TaskID);

            bool isFound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        /// <summary>
        /// Inserts a new Task into the database and returns the generated TaskID.
        /// </summary>
        public static bool AddTask(ref int TaskID, string Name, string Description,
                                   short Status, short PriorityLevel,
                                   DateTime DueDate, int TaskTypeID)
        {
            TaskID = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO [dbo].[Tasks] ([Name],[Description],[Status],[PriorityLevel],[DueDate],[TaskTypeID])
                             VALUES (@Name,@Description,@Status,@PriorityLevel,@DueDate,@TaskTypeID);
                             SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(Description) ? (object)Description : DBNull.Value);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@PriorityLevel", PriorityLevel != -1 ? (object)PriorityLevel : DBNull.Value);
            command.Parameters.AddWithValue("@DueDate", DueDate != DateTime.MinValue ? (object)DueDate : DBNull.Value);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    int.TryParse(result.ToString(), out TaskID);
            }
            finally
            {
                connection.Close();
            }

            return TaskID != -1;
        }

        /// <summary>
        /// Updates an existing task. Returns true if at least one row was affected.
        /// </summary>
        public static bool UpdateTask(int TaskID, string Name, string Description,
                                      short Status, short PriorityLevel,
                                      DateTime DueDate, int TaskTypeID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE [dbo].[Tasks]
                             SET [Name] = @Name, [Description] = @Description,
                                 [Status] = @Status, [PriorityLevel] = @PriorityLevel,
                                 [DueDate] = @DueDate, [TaskTypeID] = @TaskTypeID
                             WHERE TaskID = @TaskID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", TaskID);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(Description) ? (object)Description : DBNull.Value);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@PriorityLevel", PriorityLevel != -1 ? (object)PriorityLevel : DBNull.Value);
            command.Parameters.AddWithValue("@DueDate", DueDate != DateTime.MinValue ? (object)DueDate : DBNull.Value);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Deletes a task by TaskID.
        /// Returns true if deletion affected at least one row.
        /// </summary>
        public static bool DeleteTask(int TaskID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"DELETE FROM [dbo].[Tasks] WHERE TaskID = @TaskID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", TaskID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Retrieves all tasks for a specific TaskTypeID as a DataTable.
        /// </summary>
        public static DataTable GetTasks(int TaskTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM [dbo].[Tasks] WHERE TaskTypeID = @TaskTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);

            DataTable Tasks = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    Tasks.Load(reader);
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return Tasks;
        }
    }
}