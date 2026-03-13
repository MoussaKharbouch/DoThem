using System;
using System.Data;
using System.Data.SqlClient;

namespace DATA_LAYER
{
    /// <summary>
    /// Provides data access methods for managing Task Types in the database.
    /// Handles CRUD operations and retrieval for TaskType entities.
    /// </summary>
    public class TaskTypesData_PR
    {
        /// <summary>
        /// Retrieves a task type by its ID, populating all related fields if found.
        /// </summary>
        public static void FindTaskType(int TaskTypeID, ref string Name, ref DateTime DateOfCreation,
                                        ref string Color, ref string Description, ref int UserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TaskTypes WHERE TaskTypeID = @TaskTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Name = reader["Name"].ToString();
                    DateOfCreation = reader["DateOfCreation"] != DBNull.Value ? (DateTime)reader["DateOfCreation"] : DateTime.MinValue;
                    Color = reader["Color"] != DBNull.Value ? reader["Color"].ToString() : string.Empty;
                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                    UserID = reader["UserID"] != DBNull.Value ? int.Parse(reader["UserID"].ToString()) : -1;
                    reader.Close();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieves a task type by its Name, populating all related fields if found.
        /// </summary>
        public static void FindTaskType(string Name, ref int TaskTypeID, ref DateTime DateOfCreation,
                                        ref string Color, ref string Description, ref int UserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TaskTypes WHERE Name = @Name";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", Name);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TaskTypeID = int.Parse(reader["TaskTypeID"].ToString());
                    DateOfCreation = reader["DateOfCreation"] != DBNull.Value ? (DateTime)reader["DateOfCreation"] : DateTime.MinValue;
                    Color = reader["Color"] != DBNull.Value ? reader["Color"].ToString() : string.Empty;
                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                    UserID = reader["UserID"] != DBNull.Value ? int.Parse(reader["UserID"].ToString()) : -1;
                    reader.Close();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Checks whether a task type exists by its ID.
        /// Returns true if the record exists.
        /// </summary>
        public static bool DoesTaskTypeExist(int TaskTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM TaskTypes WHERE TaskTypeID = @TaskTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);

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
        /// Inserts a new Task Type into the database and returns the generated ID.
        /// </summary>
        public static bool AddTaskType(ref int TaskTypeID, string Name, DateTime DateOfCreation,
                                       string Color, string Description, int UserID)
        {
            TaskTypeID = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO [dbo].[TaskTypes] ([Name],[DateOfCreation],[Color],[Description],[UserID])
                             VALUES (@Name,@DateOfCreation,@Color,@Description,@UserID);
                             SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@DateOfCreation", DateOfCreation != DateTime.MinValue ? (object)DateOfCreation : DBNull.Value);
            command.Parameters.AddWithValue("@Color", !string.IsNullOrEmpty(Color) ? (object)Color : DBNull.Value);
            command.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(Description) ? (object)Description : DBNull.Value);
            command.Parameters.AddWithValue("@UserID", UserID != -1 ? (object)UserID : DBNull.Value);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    int.TryParse(result.ToString(), out TaskTypeID);
            }
            finally
            {
                connection.Close();
            }

            return TaskTypeID != -1;
        }

        /// <summary>
        /// Updates an existing Task Type.
        /// Returns true if the update affected at least one row.
        /// </summary>
        public static bool UpdateTaskType(int TaskTypeID, string Name, DateTime DateOfCreation,
                                          string Color, string Description, int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE [dbo].[TaskTypes]
                             SET [Name] = @Name, [DateOfCreation] = @DateOfCreation,
                                 [Color] = @Color, [Description] = @Description,
                                 [UserID] = @UserID
                             WHERE TaskTypeID = @TaskTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@DateOfCreation", DateOfCreation != DateTime.MinValue ? (object)DateOfCreation : DBNull.Value);
            command.Parameters.AddWithValue("@Color", !string.IsNullOrEmpty(Color) ? (object)Color : DBNull.Value);
            command.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(Description) ? (object)Description : DBNull.Value);
            command.Parameters.AddWithValue("@UserID", UserID != -1 ? (object)UserID : DBNull.Value);

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
        /// Deletes a Task Type and all associated Tasks in a single transaction.
        /// </summary>
        public static bool DeleteTaskType(int TaskTypeID)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmdTasks = new SqlCommand("DELETE FROM [dbo].[Tasks] WHERE TaskTypeID = @TaskTypeID", connection, transaction))
                    {
                        cmdTasks.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);
                        rowsAffected += cmdTasks.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdType = new SqlCommand("DELETE FROM [dbo].[TaskTypes] WHERE TaskTypeID = @TaskTypeID", connection, transaction))
                    {
                        cmdType.Parameters.AddWithValue("@TaskTypeID", TaskTypeID);
                        rowsAffected += cmdType.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Retrieves all Task Types for a given user as a DataTable.
        /// </summary>
        public static DataTable GetTaskTypes(int UserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM [dbo].[TaskTypes] WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            DataTable TaskTypes = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    TaskTypes.Load(reader);
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return TaskTypes;
        }
    }
}