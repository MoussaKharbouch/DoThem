using System;
using System.Collections.Generic;
using DoThem.Domain;

namespace DoThem.Infrastructure
{

    public interface IUserRepository
    {

        /// <summary>
        /// connect to the database by connection string
        /// <summary>
        bool ConnectToDatabase(string connectionString);

        /// <summary>
        /// find user by user id
        /// </summary>
        bool FindUser(int userID, User user);

        /// <summary>
        /// find user by username and password
        /// </summary>
        bool FindUser(string username, string password, User user);

        /// <summary>
        /// add new user
        /// </summary>
        /// <param name "newUserID"> to get id of the new user if it is added properly </param>
        bool AddUser(User user, out int newUserID);

        /// <summary>
        /// update user
        /// </summary>
        bool UpdateUser(int UserID, User newUser);

        /// <summary>
        /// delete user
        /// </summary>
        bool DeleteUser(int userID);

        /// <summary>
        /// get users with all fields
        /// </summary>
        List<User> GetUsers();

    }

}