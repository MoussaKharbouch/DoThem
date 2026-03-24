using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using DoThem.Infrastructure;
using DoThem.Domain;

namespace DoThem.Services;

public class UserService : IUserService
{

    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public string HashPassword(string password)
    {
        using SHA256 sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private void ValidateCredentials(string username, string password)
    {

        // validate username
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentNullException("Username cannot be empty or null.");
        if (username.Length > 100)
            throw new ArgumentException("Username cannot be longer than 100 characters.");
        if (username.Any(char.IsWhiteSpace))
            throw new ArgumentException("Username cannot have space.");

        // validate password
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException("Password cannot be empty or null.");
        if (password.Length > 50)
            throw new ArgumentException("Password cannot be longer than 50 characters.");

    }

    public User? FindUser(int userID)
    {

        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        return userRepository.FindUser(userID);

    }

    public User? FindUser(string username, string password)
    {

        ValidateCredentials(username, password);

        return userRepository.FindUser(username, HashPassword(password));

    }

    public User.UserStatus? GetUserStatus(int userID)
    {

        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        return userRepository.GetUserStatus(userID);

    }

    public User.UserStatus? GetUserStatus(string username, string password)
    {

        ValidateCredentials(username, password);

        return userRepository.GetUserStatus(username, HashPassword(password));

    }

    public bool DoesUserExist(int userID)
    {

        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        return userRepository.DoesUserExist(userID);

    }

    public bool DoesUserExist(string username, string password)
    {

        ValidateCredentials(username, password);

        return userRepository.DoesUserExist(username, HashPassword(password));

    }

    public int? AddUser(User user)
    {
        return userRepository.AddUser(user);
    }

    public bool UpdateUser(int userID, User newUser)
    {
        return userRepository.UpdateUser(userID, newUser);
    }

    public bool DeleteUser(int userID)
    {
        return userRepository.DeleteUser(userID);
    }

    public List<User> GetUsers()
    {
        return userRepository.GetUsers();
    }

    public bool RegisterUser(User user)
    {
        user.PasswordHash = HashPassword(user.PasswordHash);
        return userRepository.AddUser(user) != int.MinValue;
    }

    public bool Login(string username, string password)
    {
        return DoesUserExist(username, password);
    }

    public bool ChangeUserStatus(int userID, User.UserStatus status)
    {

        User? user = FindUser(userID);

        if (user != null)
        {

            user.Status = status;

            try
            {
                if (userRepository.UpdateUser(userID, user))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user to change status.", ex);
            }

        }

        return false;

    }

    public bool ChangeUserPassword(int userID, string newPassword)
    {

        // validate password
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentNullException("Password cannot be empty or null.");
        if (newPassword.Length > 50)
            throw new ArgumentException("Password cannot be longer than 50 characters.");

        User? user = FindUser(userID);

        if (user != null)
        {

            user.PasswordHash = HashPassword(newPassword);

            try
            {
                if (userRepository.UpdateUser(userID, user))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user to change password.", ex);
            }

        }

        return false;

    }

}