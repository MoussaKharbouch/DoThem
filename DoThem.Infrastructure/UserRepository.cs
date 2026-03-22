using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public class UserRepository : IUserRepository
{

    /// <summary>
    /// the connection string is private to keep it safe,
    /// it is like the password of database
    /// </summary>
    private string _ConnectionString = string.Empty;

    /// <summary>
    /// a constructor that takes the connection screen from presentation layer
    /// </summary>
    public UserRepository(string connectionString)
    {
        this._ConnectionString = connectionString;
    }

    public User? FindUser(int userID)
    {

        // query to retrieve data using sql statement with user id
        string query = @"SELECT * FROM Users
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
                SqlCommand command = new SqlCommand(query, connection);
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

                        /// make sure that the values are not null,
                        /// and if they are we return an exception, 
                        /// using null-coalescing
                        string username = reader["Username"]?.ToString() ?? throw new Exception("Username null");
                        string email = reader["Email"]?.ToString() ?? throw new Exception("Email null");
                        string password = reader["Password"]?.ToString() ?? throw new Exception("Password null");

                        // make sure that the user status is in the right range
                        User.UserStatus status = Enum.IsDefined(typeof(User.UserStatus), Convert.ToInt32(reader["Status"])) ? (User.UserStatus)Convert.ToInt32(reader["Status"]) : User.UserStatus.Expired;

                        return new User(
                            UserID: Convert.ToInt32(reader["UserID"]),
                            Username: username,
                            Email: email,
                            PasswordHash: password,
                            CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                            Status: (User.UserStatus)Convert.ToInt32(reader["Status"])
                        );

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Finding user failed.", ex);
        }

        return null;

    }

    public User? FindUser(string username, string password)
    {

        // query to retrieve data using sql statement with user id
        string query = @"SELECT * FROM Users
                        Where Username = @Username and Password = @Password";

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
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                // use reader to get data from database
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    /// if reader doesn't have any rows,
                    /// we return null
                    if (!reader.HasRows)
                        return null;

                    if (reader.Read())
                    {

                        /// make sure that the values are not null,
                        /// and if they are we return an exception, 
                        /// using null-coalescing
                        /// we don't check username and password because they are alreade the paameters,
                        /// so they cannot be null in database
                        string email = reader["Email"]?.ToString() ?? throw new Exception("Email null");

                        // make sure that the user status is in the right range
                        User.UserStatus status = Enum.IsDefined(typeof(User.UserStatus), Convert.ToInt32(reader["Status"])) ? (User.UserStatus)Convert.ToInt32(reader["Status"]) : User.UserStatus.Expired;

                        return new User(
                            UserID: Convert.ToInt32(reader["UserID"]),
                            Username: reader["Username"].ToString()!,
                            Email: email,
                            PasswordHash: reader["Password"].ToString()!,
                            CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                            Status: (User.UserStatus)Convert.ToInt32(reader["Status"])
                        );

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Finding user failed.", ex);
        }

        return null;

    }

    public User.UserStatus? GetUserStatus(int userID)
    {

        // query to retrieve data using sql statement with user id
        string query = @"SELECT Status FROM Users
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
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                // use scalar to retrieve single value from database
                object result = command.ExecuteScalar();

                if(result != null && result != DBNull.Value)
                    return (User.UserStatus)Convert.ToInt32(result);
                else
                    return null;

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting user's status failed.", ex);
        }

    }

    public User.UserStatus? GetUserStatus(string username, string password)
    {

        // query to retrieve data using sql statement with user id
        string query = @"SELECT Status FROM Users
                        Where Username = @Username and Password = @Password";

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
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                // use scalar to retrieve single value from database
                object result = command.ExecuteScalar();

                if(result != null && result != DBNull.Value)
                    return (User.UserStatus)Convert.ToInt32(result);
                else
                    return null;

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting user's status failed.", ex);
        }

    }

    public bool DoesUserExist(int userID)
    {
        
        // query to retrieve data using sql statement with user id
        string query = @"SELECT 1 FROM Users
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
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                // use scalar to retrieve single value from database
                object result = command.ExecuteScalar();
                return (result != null && result != DBNull.Value);

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Checking if user exists failed.", ex);
        }

    }

    public bool DoesUserExist(string username, string password)
    {

        // query to retrieve data using sql statement with user id
        string query = @"SELECT 1 FROM Users
                        Where Username = @Username and Password = @Password";

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
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                // use scalar to retrieve single value from database
                object result = command.ExecuteScalar();
                return (result != null && result != DBNull.Value);

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting user's status failed.", ex);
        }

    }

    public int AddUser(User user)
    {
        return int.MinValue;
    }

    public bool UpdateUser(int userID, User newUser)
    {
        return false;
    }

    public bool ChangeUserStatus(int userID, User.UserStatus status)
    {
        return false;
    }

    public bool DeleteUser(int userID)
    {
        return false;
    }

    public List<User> GetUsers()
    {
        return new List<User>();
    }

}