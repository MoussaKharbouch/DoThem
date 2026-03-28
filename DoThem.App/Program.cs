using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using DoThem.Domain;
using DoThem.Services;
using DoThem.Infrastructure;
using Microsoft.IdentityModel.Protocols;
using System.Data;
using Microsoft.Identity.Client;

namespace DoThem.App
{
    internal class Program
    {

        // not all methods have testing functions, just the big ones or the ones that have big logic

        /// <summary>
        /// in case you don't know a password in hash :)
        /// you can use this function to hash a password and then use the hash in the database,
        /// and then you can use the same password to login,
        /// because the service will hash it and compare it with the hash in the database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        static public string HashPassword(string password)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Prints user's information with colors
        /// </summary>
        public static void PrintUserInfo(User? user)
        {

            if (user == null)
            {
                Console.WriteLine("User is null.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===== User Information =====");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("UserID: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(user!.UserID);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Username: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(user.Username);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Email: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(user.Email);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("PasswordHash: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(user.Password);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("CreationDate: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(user.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Status: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(user.Status);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================");

            Console.ResetColor();
        }

        static void TestFindUserFunction(int userID, IUserRepository userRepository)
        {

            User? user = null;

            try
            {
                user = new UserService(userRepository).FindUser(userID);
                PrintUserInfo(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        static void TestFindUserFunction(string username, string password, IUserRepository userRepository)
        {

            User? user = null;

            try
            {
                user = new UserService(userRepository).FindUser(username, password);
                PrintUserInfo(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public static void TestUserPart(IUserRepository userRepository)
        {

            // we create the service here because we need it for all the tests, and we don't want to create it in each test function
            UserService userService = new UserService(userRepository);

            // we will test all the functions of the user service, and we will print the results with colors for better visualization
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n======================================");
            Console.WriteLine("         USER MODULE TEST");
            Console.WriteLine("======================================\n");
            Console.ResetColor();

            int? newUserID = int.MinValue; // we will use this variable to store the new user id, and we will use it in the next tests

            try
            {

                /// 1. Add User
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(">> [1] Adding New User...");
                Console.ResetColor();

                User newUser = new User(
                    1,
                    "MoussaDev",
                    "moussa5arbouche@gmail.com",
                    "password_123",
                    new DateTime(2023, 5, 10),
                    User.UserStatus.Active
                );

                newUserID = userService.AddUser(newUser);

                if (newUserID == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Failed to add user.\n");
                    Console.ResetColor();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✔ User added successfully with ID = {newUserID}\n");
                Console.ResetColor();


                // 2. Find User by ID
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(">> [2] Finding User by ID...");
                Console.ResetColor();

                var userById = userService.FindUser((int)newUserID);
                PrintUserInfo(userById);


                // 3. Find User by Username & Password
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [3] Finding User by Username & Password...");
                Console.ResetColor();

                var userByCredentials = userService.FindUser("MoussaDev", "password_123");
                PrintUserInfo(userByCredentials);

                // 4. Check if user exists
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [4] Checking if user exists...");
                Console.ResetColor();

                bool exists = userService.DoesUserExist((int)newUserID);
                Console.WriteLine($"User Exists (by ID): {exists}");

                // 5. Get User Status
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [5] Getting user status...");
                Console.ResetColor();

                var status = userService.GetUserStatus((int)newUserID);
                Console.WriteLine($"User Status (by ID): {status}");

                status = userService.GetUserStatus("MoussaDev", "password_123");
                Console.WriteLine($"User Status (by credentials): {status}");

                // 6. Update User
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [6] Updating user information...");
                Console.ResetColor();
                User updatedUser = new User(
                    (int)newUserID,
                    "MoussaDevUpdated",
                    "moussa5arbouche@gmail.com",
                    "updated_password_123",
                    new DateTime(2023, 5, 10),
                    User.UserStatus.Active
                );

                bool updated = userService.UpdateUser((int)newUserID, updatedUser);
                Console.WriteLine($"User Updated: {updated}");

                // 7. Change User Status
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [7] Changing user status...");
                Console.ResetColor();
                bool statusChanged = userService.ChangeUserStatus((int)newUserID, User.UserStatus.Banned);
                Console.WriteLine($"User Status Changed: {statusChanged}");

                // 8. Get All Users
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [6] Retrieving all users...");
                Console.ResetColor();

                var users = userService.GetUsers();

                if (users.Count == 0)
                {
                    Console.WriteLine("⚠ No users found.");
                }
                else
                {
                    Console.WriteLine($"✔ Total Users: {users.Count}\n");

                    foreach (var user in users)
                    {
                        PrintUserInfo(user);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ ALL TESTS COMPLETED SUCCESSFULLY!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ ERROR OCCURRED:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n======================================\n");
            Console.ResetColor();

            /// 9. Delete User - we will delete the user we just created, and then we will show user list to show that the user is deleted
            /// we delete user because username is unique, so if we run the program again it will throw an exception,
            /// because we will try to add a user with the same username,
            /// so we delete the user to avoid this problem, and also to test the delete function
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">> [8] Deleting user...");
            Console.ResetColor();
            bool deleted = userService.DeleteUser((int)newUserID!); // we are sure that newUserID is not null because if it was null we would have returned from the function in the catch block, so we can safely cast it to int
            Console.WriteLine($"User Deleted: {deleted}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n>> [9] Retrieving all users after deletion...");
            Console.ResetColor();
            List<User> usersAfterDeletion = userService.GetUsers();
            if (usersAfterDeletion.Count == 0)
            {
                Console.WriteLine("⚠ No users found.");
            }
            else
            {
                Console.WriteLine($"✔ Total Users: {usersAfterDeletion.Count}\n");

                foreach (var user in usersAfterDeletion)
                {
                    PrintUserInfo(user);
                }

            }

        }

        static void Main(string[] args)
        {

            /// we used independency injection so the user service can use any repository
            /// that's really important, by changing repository we can change how we deal with data
            /// we can use another RDBMS such as: sqllite, postgresql...
            /// and we can use another provider such as: ef core

            /// new user repository, we get the connection string from app.config for security
            /// we have used configuration manager, you have to download some packages
            IUserRepository userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["DothemDB"].ToString());
            TestUserPart(userRepository);

            Console.WriteLine("Press Any Key To Close...");
            Console.Read();

        }

    }

}