using System;
using System.Linq;

namespace DoThem.Domain;

public class User
{

    //UserID is the primary key for users's table
    public int UserID { get; private set; }

    //Username must be short and unique
    private string _Username = string.Empty;
    public string Username
    {

        get
        {
            return _Username;
        }

        set
        {

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Username cannot be empty or null.");
            if (value.Length > 100)
                throw new ArgumentException("Username cannot be longer than 100 characters.");
            if (value.Any(char.IsWhiteSpace))
                throw new ArgumentException("Username cannot have space.");

            _Username = value;

        }

    }

    //Email can be used once in the system else if the account is expired
    private string _Email = string.Empty;
    public string Email
    {

        get
        {
            return _Email;
        }

        set
        {

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Email cannot be empty or null.");
            if (!value.EndsWith("@gmail.com"))
                throw new ArgumentException(nameof(value), "Invalid email format.");
            if (value.Length > 100)
                throw new ArgumentException("Email cannot be longer than 100 characters.");
            if (value.Any(char.IsWhiteSpace))
                throw new ArgumentException("Email cannot have space.");

            _Email = value;

        }

    }

    //Password must be encrypted so no one can see it
    private string _Password = string.Empty;
    public string Password
    {

        get
        {
            return _Password;
        }

        set
        {

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Password cannot be empty or null.");

            if (value.Length > 50)
                throw new ArgumentException("Password cannot be longer than 50 characters.");

            _Password = value;

        }

    }

    private DateTime _CreationDate;
    private static readonly DateTime MinCreationDate = new DateTime(2000, 1, 1);

    public DateTime CreationDate {

        get
        {
            return _CreationDate;
        }

        private set
        {

            if(value > DateTime.UtcNow)
                throw new ArgumentException("Creation date cannot be in the future.");
            if(value < MinCreationDate)
                throw new ArgumentException("Creation date cannot be older than the year of 2000.");

            _CreationDate = value;

        }

    }

    public enum UserStatus { Active = 1, Expired = 2, Banned = 3 }
    public UserStatus Status { get; set; }

    public User(int UserID, string Username, string Email, string PasswordHash, DateTime CreationDate, UserStatus Status)
    {
        this.UserID = UserID;
        this.Username = Username;
        this.Email = Email;
        this.Password = PasswordHash;
        this.CreationDate = CreationDate;
        this.Status = Status;
    }

}

