using ChatApp_SignalR.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatApp_SignalR.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //To close the application
            Application.Current.Shutdown();
        }

        private void btn_LoginClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTB.Text.Trim();
            string password = PasswordTB.Password;
            // validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorMessageLabel.Content = "Please fill out all fields.";
                return;
            }

            string connectionString = App.GetConnectString();
            string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // user exists and password matches - login successful
                            var user = new Users
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                ProfilePicture = (byte[])reader["ProfilePicture"]
                            };

                            var mainWindow = new MainWindow(user);
                            mainWindow.Show();

                            // close login window if needed
                            Close();
                        }
                        else
                        {
                            ErrorMessageLabel.Content = "Invalid username or password.";
                            return;
                        }
                    }
                }
            }
        }
        private void btn_SignupClick(object sender, RoutedEventArgs e)
        {
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.Show();
            this.Close();
        }
    }
}
