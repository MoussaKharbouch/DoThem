using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using DoThem.Domain;
using System.Data;

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
                            Status: status
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
                            Status: status
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

                if (result != null && result != DBNull.Value)
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

                if (result != null && result != DBNull.Value)
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
            throw new Exception("Checking if user exists failed failed.", ex);
        }

    }

    public int? AddUser(User user)
    {

        // the query that add a new user to database (at the end, we get the id of the new user)
        string query = @"INSERT INTO [dbo].[Users]
                                ([Username]
                                ,[Email]
                                ,[Password]
                                ,[CreationDate]
                                ,[Status])
                            VALUES
                                (@Username
                                @Email
                                @Password
                                @CreationDate
                                @Status)";

        try
        {

            /// connecting to string using SqlConnection with the connection string
            /// we add using for resource management
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {

                // command to add new user (we add all parameters from the object)
                SqlCommand command = new SqlCommand(query, connection);

                // add the parameters
                command.Parameters.AddWithValue("", user.Username);
                command.Parameters.AddWithValue("", user.Email);
                command.Parameters.AddWithValue("", user.PasswordHash);
                command.Parameters.AddWithValue("", user.CreationDate);
                command.Parameters.AddWithValue("", user.Status);

                // we retrieve the id of the added user
                object result = command.ExecuteScalar();
                int newUserID;

                // checking if the value is valide (it can return nothing)
                if (result != null && int.TryParse(result.ToString(), out newUserID))
                    return newUserID;
                else
                    return null;

            }

        }
        catch (Exception ex)
        {
            throw new Exception("Adding new user failed", ex);
        }

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

        List<User> Users = new List<User>();

        // query to retrieve data using sql statement with user id
        string query = @"SELECT * FROM Users";

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

                // use reader to get data from database
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    /// if reader doesn't have any rows,
                    /// we return the list directly
                    if (!reader.HasRows)
                        return Users;

                    while (reader.Read())
                    {

                        /// make sure that the values are not null,
                        /// and if they are we return an exception, 
                        /// using null-coalescing
                        string username = reader["Username"]?.ToString() ?? throw new Exception("Username null");
                        string email = reader["Email"]?.ToString() ?? throw new Exception("Email null");
                        string password = reader["Password"]?.ToString() ?? throw new Exception("Password null");

                        // make sure that the user status is in the right range
                        User.UserStatus status = Enum.IsDefined(typeof(User.UserStatus), Convert.ToInt32(reader["Status"])) ? (User.UserStatus)Convert.ToInt32(reader["Status"]) : User.UserStatus.Expired;

                        // Defining user from current row in database
                        User user = new User(
                            UserID: Convert.ToInt32(reader["UserID"]),
                            Username: username,
                            Email: email,
                            PasswordHash: password,
                            CreationDate: Convert.ToDateTime(reader["CreationDate"]),
                            Status: (User.UserStatus)Convert.ToInt32(reader["Status"])
                        );

                        Users.Add(user);

                    }

                }

            }

        }
        catch (SqlException ex)
        {
            throw new Exception("Getting users failed.", ex);
        }

        return Users;

    }

}