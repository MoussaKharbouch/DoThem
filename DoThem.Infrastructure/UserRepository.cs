using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public class UserRepository : IUserRepository
{

    public User? FindUser(int userID)
    {
        return new User("", "", "", DateTime.Now, User.UserStatus.Expired);
    }

    public User? FindUser(string username, string password)
    {
        return new User("", "", "", DateTime.Now, User.UserStatus.Expired);
    }

    public User.UserStatus? GetUserStatus(int userID)
    {
        return User.UserStatus.Expired;
    }

    public User.UserStatus? GetUserStatus(string username, string password)
    {
        return User.UserStatus.Expired;
    }

    public bool DoesUserExist(int userID)
    {
        return false;
    }

    public bool DoesUserExist(string username, string password)
    {
        return false;
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