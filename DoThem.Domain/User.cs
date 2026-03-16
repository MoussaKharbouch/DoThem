using System;

namespace DoThem.Domain
{
    
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

                _Email = value;

            }

        }

        //Password must be encrypted so no one can see it
        private string _PasswordHash = string.Empty;
        public string PasswordHash
        {

            get
            {
                return _PasswordHash;
            }

            set
            {

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Password cannot be empty or null.");

                if (value.Length > 50)
                    throw new ArgumentException("Password cannot be longer than 50 characters.");

                _PasswordHash = value;

            }

        }

        public DateTime CreationDate { get; private set; }

        public enum UserStatus { Active = 1, Expired = 2, Banned = 3 }
        public UserStatus Status { get; set; }

        public User(string Username, string Email, string PasswordHash, DateTime CreationDate, UserStatus Status)
        {
            this.Username = Username;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.CreationDate = CreationDate;
            this.Status = Status;
        }

    }

}