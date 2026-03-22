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
            Console.WriteLine(user.PasswordHash);

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

            /// --------------------------------------------------------------
            /// Users testing part
            /// (you must define the data inside the database so the functions don't return null)

            /// TestFindUserFunction(5, userRepository);
            /// TestFindUserFunction("John", "example", userRepository);
            TestFindUserFunction("Supremekai", "example", userRepository);

            UserService userService = new UserService(userRepository);

            /// Console.WriteLine(userService.GetUserStatus(6));
            /// Console.WriteLine(userService.GetUserStatus("Cancelo", "example"));

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