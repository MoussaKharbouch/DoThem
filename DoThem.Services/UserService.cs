using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using DoThem.Infrastructure;
using DoThem.Domain;
using System.Data;

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

    private void ValidateUserFields(User user)
    {

        ValidateCredentials(user.Username, user.Password);

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentNullException("Email cannot be empty or null.");
        if (!user.Email.EndsWith("@gmail.com"))
            throw new ArgumentException(nameof(user.Email), "Invalid email format.");
        if (user.Email.Length > 100)
            throw new ArgumentException("Email cannot be longer than 100 characters.");
        if (user.Email.Any(char.IsWhiteSpace))
            throw new ArgumentException("Email cannot have space.");

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
        /// repository doesn't search with password,
        /// it searches with username only,
        /// so we will search with username and then check the password in the service layer,
        /// if the password is correct we will return the user, otherwise we will return null
        User? user = userRepository.FindUser(username);
        if (user != null && user.Password == HashPassword(password))
            return user;
        return null;
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
        /// repository doesn't search with password,
        /// it searches with username only,
        /// so we will search with username and then check the password in the service layer,
        /// if the password is correct we will return the user status, otherwise we will return null
        User? user = userRepository.FindUser(username);
        if (user != null && user.Password == HashPassword(password))
            return user.Status;
        return null;
    }

    public bool DoesUserExist(int userID)
    {
        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");
        return userRepository.DoesUserExist(userID);
    }

    // here we will check if the username already exists in the system,
    // and if email already exists in the system
    public int? AddUser(User user)
    {

        ValidateUserFields(user);

        if(userRepository.IsUsernameUsed(user.Username))
            throw new ArgumentException("Username already exists in the system.");
        if(userRepository.IsEmailUsed(user.Email))
            throw new ArgumentException("Email already exists in the system.");

        user.Password = HashPassword(user.Password);
        return userRepository.AddUser(user);

    }

    // here we will check if the username already exists in the system,
    // and if email already exists in the system
    public bool UpdateUser(int userID, User newUser)
    {

        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        ValidateUserFields(newUser);
    
        // we can't hash the password, maybe the user doesn't want to change it, so we will check if the password is the same as the old one, if it is, we will keep it, otherwise we will hash it
        User? oldUser = FindUser(userID);
        if (oldUser == null)
            throw new ArgumentException("User with the given ID does not exist.");
        if (oldUser.Username != newUser.Username && userRepository.IsUsernameUsed(newUser.Username))
            throw new ArgumentException("Username already exists in the system.");
        if (oldUser.Email != newUser.Email && userRepository.IsEmailUsed(newUser.Email))
            throw new ArgumentException("Email already exists in the system.");
        if (oldUser.Password != newUser.Password)
            newUser.Password = HashPassword(newUser.Password);

        return userRepository.UpdateUser(userID, newUser);

    }

    public bool DeleteUser(int userID)
    {
        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");
        return userRepository.DeleteUser(userID);
    }

    public List<User> GetUsers()
    {
        return userRepository.GetUsers();
    }

    public bool RegisterUser(User user)
    {
        user.Password = HashPassword(user.Password);
        return userRepository.AddUser(user) != null;
    }

    public bool Login(string username, string password)
    {

        ValidateCredentials(username, password);

        // check if user is active, if it is not active we will return false, if it is active we will check the credentials and return true if they are correct, otherwise we will return false
        User? user = FindUser(username, password);
        if (user == null)
            return false;
        if (user.Status != User.UserStatus.Active)
            return false;

        return true;

    }

    public bool ChangeUserStatus(int userID, User.UserStatus status)
    {

        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        // we change status not with update user, we change it with changeuserstatus method in repository, but we will check if the user exists first, if it doesn't exist we will return false, if it exists we will change the status and then update the user
        // check if user id is negative
        if (userID < 0)
            throw new ArgumentOutOfRangeException("user id cannot be negative.");

        User? user = FindUser(userID);

        if (user != null)
        {

            user.Status = status;

            try
            {
                if (userRepository.ChangeUserStatus(userID, status))
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

            user.Password = HashPassword(newPassword);

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