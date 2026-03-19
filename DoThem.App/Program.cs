using System;
using System.Configuration;
using DoThem.Domain;
using DoThem.Services;
using DoThem.Infrastructure;
using Microsoft.IdentityModel.Protocols;
using System.Data;

namespace DoThem.App
{
    internal class Program
    {
        /// <summary>
        /// Prints user's information with colors
        /// </summary>
        public static void PrintInfo(User? user)
        {

            if(user == null)
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

        static void TestFindUserFunction(int userID)
        {

            IUserRepository userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["DothemDB"].ToString());

            User? user = null;

            try
            {
                user = new UserService(userRepository).FindUser(userID);
                PrintInfo(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        static void Main(string[] args)
        {

            TestFindUserFunction(1);

            Console.WriteLine("Press Any Key To Close...");
            Console.Read();

        }

    }

}