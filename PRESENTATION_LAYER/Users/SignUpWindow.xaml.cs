using System;
using System.Windows;
using BUSINESS_LAYER;

namespace PRESENTATION_LAYER.Users
{
    /// <summary>
    /// Represents the Sign Up window.
    /// مسؤول عن إنشاء مستخدم جديد وتسجيل دخوله مباشرة بعد الإنشاء.
    /// </summary>
    public partial class SignUpWindow : Window
    {
        /// <summary>
        /// Initializes the SignUp window.
        /// </summary>
        public SignUpWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validates user input fields.
        /// Ensures:
        /// - Username is not empty
        /// - Password is not empty
        /// - Password and Confirm Password match
        /// </summary>
        /// <returns>
        /// True if input is valid; otherwise false.
        /// </returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(tbUsername.Text)) return false;
            if (string.IsNullOrEmpty(pbPassword.Password)) return false;
            if (pbPassword.Password != pbConfirmPassword.Password) return false;
            return true;
        }

        /// <summary>
        /// Handles Sign Up button click event.
        /// Creates new user and logs them in if successful.
        /// </summary>
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            // Validate form input
            if (!ValidateInput())
            {
                MessageBox.Show("Please fill all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new user using entered information
            User NewUser = new User
            {
                Username = tbUsername.Text,
                Password = pbPassword.Password
            };

            // Attempt to save user
            if (NewUser.Save())
            {
                // Retrieve the newly created user from database
                General.User = User.FindUser(NewUser.Username, NewUser.Password);

                if (General.User != null)
                {
                    // Add default task type so user can use it immediately
                    clsTaskType TaskType = new clsTaskType
                    {
                        Name = "General",
                        Description = "General tasks",
                        Color = "#FFCCCCCC",
                        UserID = General.User.UserID
                    };

                    TaskType.Save();

                    MessageBox.Show("User created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Open main window (automatic login)
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error retrieving user after creation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Error creating user. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Navigates back to Login window when user clicks
        /// the "Back to Login" text.
        /// </summary>
        private void tbBackToLogin_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoginWindow LoginWindow = new LoginWindow();
            LoginWindow.Show();
            this.Close();
        }
    }
}