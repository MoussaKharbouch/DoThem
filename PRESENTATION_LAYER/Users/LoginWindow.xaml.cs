using System;
using System.Windows;
using System.Windows.Input;
using BUSINESS_LAYER;

namespace PRESENTATION_LAYER.Users
{
    /// <summary>
    /// Represents the Login window.
    /// Handles user authentication and navigation to main application.
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Initializes the Login window.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles Enter button click event.
        /// </summary>
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            Login(tbUsername.Text, tbPassword.Password);
        }

        /// <summary>
        /// Attempts to authenticate user with provided credentials.
        /// Validates account status before allowing access.
        /// </summary>
        /// <param name="Username">Entered username</param>
        /// <param name="Password">Entered password</param>
        private void Login(string Username, string Password)
        {
            // Retrieve user from Business Layer
            User user = User.FindUser(Username, Password);

            // If user not found
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            // Check account status
            if (user.Status == User.enStatus.Expired)
            {
                MessageBox.Show("Your account has expired. Please contact support.");
                return;
            }
            else if (user.Status == User.enStatus.Banned)
            {
                MessageBox.Show("Your account has been banned. Please contact support.");
                return;
            }

            // Store logged-in user globally
            General.User = user;

            // Open main window
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();

            this.Close();
        }

        /// <summary>
        /// Opens SignUp window when user clicks the Sign Up text.
        /// </summary>
        private void tbSignUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Users.SignUpWindow SignUpWindow = new Users.SignUpWindow();
            SignUpWindow.Show();
            this.Close();
        }
    }
}