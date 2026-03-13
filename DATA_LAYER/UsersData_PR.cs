using System;
using System.Data;
using System.Data.SqlClient;

namespace DATA_LAYER
{
    /// <summary>
    /// Provides data access methods for managing Users in the database.
    /// Handles CRUD operations, existence checks, and data retrieval.
    /// </summary>
    public static class UsersData_PR
    {
        /// <summary>
        /// Attempts to locate a user by username and password.
        /// If found, populates the UserID and Status.
        /// </summary>
        public static void FindUser(string Username, string Password, ref int UserID, ref short Status)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM USERS WHERE Username = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];
                    Status = Convert.ToInt16(reader["status"]);
                    reader.Close();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieves user data by UserID. Returns username, password, and status if found.
        /// </summary>
        public static void FindUser(int UserID, ref string Username, ref string Password, ref short Status)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM USERS WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Username = reader["Username"].ToString();
                    Password = reader["Password"].ToString();
                    Status = Convert.ToInt16(reader["status"]);
                    reader.Close();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Checks if a user exists with the given username and password.
        /// Returns true if a matching record is found.
        /// </summary>
        public static bool DoesUserExist(string Username, string Password)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM Users WHERE Username = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);

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
        /// Verifies the existence of a user by UserID.
        /// Returns true if a record with the specified ID exists.
        /// </summary>
        public static bool DoesUserExist(int UserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM Users WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

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
        /// Determines if the specified username already exists in the database.
        /// Useful for validation before creating or updating users.
        /// </summary>
        public static bool DoesUsernameExist(string Username)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM Users WHERE Username = @Username";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);

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
        /// Determines if the specified username exists for any user other than the excluded UserID.
        /// Useful for enforcing unique usernames during updates.
        /// </summary>
        public static bool DoesUsernameExist(string Username, int ExcludedUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT 1 AS FOUND FROM Users WHERE Username = @Username AND UserID != @ExcludedUserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@ExcludedUserID", ExcludedUserID);

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
        /// Inserts a new user into the database.
        /// Returns true if the operation was successful and outputs the generated UserID.
        /// </summary>
        public static bool AddUser(ref int UserID, string Username, string Password, short Status)
        {
            UserID = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[Users] ([Username],[Password],[status])
                             VALUES (@Username,@Password,@Status);
                             SELECT SCOPE_IDENTITY() as UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Status", Status);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    int.TryParse(result.ToString(), out UserID);
            }
            finally
            {
                connection.Close();
            }

            return (UserID != -1);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// Returns true if at least one row was affected.
        /// </summary>
        public static bool UpdateUser(int UserID, string Username, string Password, short Status)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[Users]
                             SET [Username] = @Username,
                                 [Password] = @Password,
                                 [status] = @Status
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a user by UserID.
        /// Returns true if the deletion affected at least one row.
        /// </summary>
        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"DELETE FROM [dbo].[Users] WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Retrieves all users as a DataTable.
        /// Returns an empty DataTable if no records exist.
        /// </summary>
        public static DataTable GetUsers()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = "SELECT * FROM [dbo].[Users]";
            SqlCommand command = new SqlCommand(query, connection);
            DataTable Users = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    Users.Load(reader);
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return Users;
        }
    }
}