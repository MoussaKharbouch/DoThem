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

        // in case you don't know a password in hash :)
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

            UserService userService = new UserService(userRepository);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n======================================");
            Console.WriteLine("         USER MODULE TEST");
            Console.WriteLine("======================================\n");
            Console.ResetColor();

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

                int? newUserID = userService.AddUser(newUser);

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

                exists = userService.DoesUserExist("MoussaDev", "password_123");
                Console.WriteLine($"User Exists (by credentials): {exists}");


                // 5. Get User Status
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n>> [5] Getting user status...");
                Console.ResetColor();

                var status = userService.GetUserStatus((int)newUserID);
                Console.WriteLine($"User Status (by ID): {status}");

                status = userService.GetUserStatus("MoussaDev", "password_123");
                Console.WriteLine($"User Status (by credentials): {status}");


                // 6. Get All Users
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