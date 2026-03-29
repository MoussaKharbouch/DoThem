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
    /// find user by username
    /// </summary>
    User? FindUser(string username);

    /// <summary>
    /// get the user's status by user's id
    /// </summary>
    User.UserStatus? GetUserStatus(int userID);

    /// <summary>
    /// check if user exists by user's id
    /// </summary>
    bool DoesUserExist(int userID);

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
    /// change user status
    /// </summary>
    bool ChangeUserStatus(int userID, User.UserStatus status);

    /// <summary>
    /// get users with all fields
    /// </summary>
    List<User> GetUsers();

}
