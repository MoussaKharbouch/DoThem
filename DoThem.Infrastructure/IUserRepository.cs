using System;
using System.Collections.Generic;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public interface IUserRepository
{

    /// <summary>
    /// find user by user id
    /// </summary>
    User? FindUser(int userID);

    /// <summary>
    /// find user by username and password
    /// </summary>
    User? FindUser(string username, string password);

    /// <summary>
    /// get the user's status by user's id
    /// </summary>
    User.UserStatus? GetUserStatus(int userID);

    /// <summary>
    /// get the user's status by username and password
    /// </summary>
    User.UserStatus? GetUserStatus(string username, string password);

    /// <summary>
    /// check if user exists by user's id
    /// </summary>
    bool DoesUserExist(int userID);

    /// <summary>
    /// check if user exists by username and password
    /// </summary>
    bool DoesUserExist(string username, string password);

    /// <summary>
    /// add new user
    /// </summary>
    /// <returns>
    /// the id of the new user
    /// </returns>
    int? AddUser(User user);

    /// <summary>
    /// update user
    /// </summary>
    bool UpdateUser(int userID, User newUser);

    /// <summary>
    /// delete user
    /// </summary>
    bool DeleteUser(int userID);

    /// <summary>
    /// get users with all fields
    /// </summary>
    List<User> GetUsers();

}
