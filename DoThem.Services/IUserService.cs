using System;
using DoThem.Domain;

namespace DoThem.Services;

public interface IUserService
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
    /// check if user exists by user id
    /// </summary>
    bool DoesUserExist(int userID);

    /// <summary>
    /// check if user exists by username and password
    /// </summary>
    bool DoesUserExist(string username, string password);

    /// <summary>
    /// add new user
    /// </summary>
    int AddUser(User user);

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

    /// <summary>
    /// register user by add it in database
    /// </summary>
    bool RegisterUser(User user);

    /// <summary>
    /// login the user by checking if user is in the database
    /// and if it is active
    /// </summary>
    bool Login(string username, string password);

    /// <summary>
    /// change the user's status
    /// </summary>
    bool ChangeUserStatus(int userID, User.UserStatus status);

    /// <summary>
    /// change the user's password
    /// </summary>
    bool ChangeUserPassword(int userID, string newPassword);

}
