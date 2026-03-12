using System;
using System.Data;
using DATA_LAYER;

namespace BUSINESS_LAYER
{
    /// <summary>
    /// Represents a system user.
    /// Handles user-related business logic and communicates
    /// with the Data Layer.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Defines whether the object is in Add or Update mode.
        /// </summary>
        enum enMode { Add, Update }
        enMode Mode;

        /// <summary>
        /// Defines user status.
        /// </summary>
        public enum enStatus { Active = 1, Expired = 2, Banned = 3 }

        /// <summary>
        /// Primary key of the user.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Username used for login.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Current status of the user account.
        /// </summary>
        public enStatus Status { get; set; }

        /// <summary>
        /// Default constructor used when creating new user.
        /// </summary>
        public User()
        {
            UserID = -1;
            Username = string.Empty;
            Password = string.Empty;
            Status = enStatus.Active;

            Mode = enMode.Add;
        }

        /// <summary>
        /// Constructor used when loading existing user from database.
        /// </summary>
        public User(int UserID, string Username, string Password, enStatus Status)
        {
            this.UserID = UserID;
            this.Username = Username;
            this.Password = Password;
            this.Status = Status;

            Mode = enMode.Update;
        }

        /// <summary>
        /// Finds user by username and password.
        /// Returns null if not found.
        /// </summary>
        public static User FindUser(string Username, string Password)
        {
            int UserID = -1;
            short status = 1;

            UsersData.FindUser(Username, Password, ref UserID, ref status);

            if (UserID == -1)
                return null;

            return new User(UserID, Username, Password, (enStatus)status);
        }

        /// <summary>
        /// Finds user by ID.
        /// Returns null if not found.
        /// </summary>
        public static User FindUser(int UserID)
        {
            string Username = string.Empty;
            string Password = string.Empty;
            short status = 0;

            UsersData.FindUser(UserID, ref Username, ref Password, ref status);

            if (Username == string.Empty && Password == string.Empty && status == 0)
                return null;

            return new User(UserID, Username, Password, (enStatus)status);
        }

        /// <summary>
        /// Adds new user to database.
        /// </summary>
        private bool Add()
        {
            if (DoesUsernameExist(Username))
                return false;

            int UserID = this.UserID;

            bool succeeded = UsersData.AddUser(ref UserID, Username, Password, (short)Status);

            this.UserID = UserID;

            return succeeded;
        }

        /// <summary>
        /// Updates existing user in database.
        /// </summary>
        private bool Update()
        {
            if (UsersData.DoesUsernameExist(Username, UserID))
                return false;

            return UsersData.UpdateUser(UserID, Username, Password, (short)Status);
        }

        /// <summary>
        /// Saves current user (Add or Update automatically).
        /// </summary>
        public bool Save()
        {
            bool succeeded = false;

            switch (Mode)
            {
                case enMode.Add:
                    succeeded = Add();

                    if (succeeded)
                        Mode = enMode.Update;
                    break;

                case enMode.Update:
                    succeeded = Update();
                    break;
            }

            return succeeded;
        }

        /// <summary>
        /// Deletes user by ID.
        /// </summary>
        public static bool DeleteUser(int UserID)
        {
            if (!UsersData.DoesUserExist(UserID))
                return false;

            return UsersData.DeleteUser(UserID);
        }

        /// <summary>
        /// Checks if user exists by credentials.
        /// </summary>
        public static bool DoesUserExist(string Username, string Password)
        {
            return UsersData.DoesUserExist(Username, Password);
        }

        /// <summary>
        /// Checks if username already exists.
        /// </summary>
        public static bool DoesUsernameExist(string Username)
        {
            return UsersData.DoesUsernameExist(Username);
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        static public DataTable GetUsers()
        {
            return UsersData.GetUsers();
        }
    }
}