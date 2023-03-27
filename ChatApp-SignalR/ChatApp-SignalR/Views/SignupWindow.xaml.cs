using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ChatApp_SignalR.Views
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        SqlConnection connection = new SqlConnection(App.GetConnectString());
        private string avatarFilePath;
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void btn_SignupClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameTB.Text;
            string email = EmailTB.Text;
            string password = PasswordTB.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessageLabel.Content = "Please fill out all fields.";
                return;
            }
            // check if email already exists
            string connectionString = App.GetConnectString();
            string checkEmailSql = "SELECT COUNT(*) FROM Users WHERE Email = @email";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(checkEmailSql, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        ErrorMessageLabel.Content = "Email already exists.";
                        return;
                    }
                }
            }

            byte[] avatarBytes = null;
            if (!string.IsNullOrEmpty(avatarFilePath))
            {
                using (var stream = new FileStream(avatarFilePath, FileMode.Open, FileAccess.Read))
                {
                    avatarBytes = new byte[stream.Length];
                    stream.Read(avatarBytes, 0, (int)stream.Length);
                }
            }
            // save user info to database
            
            string insertSql = "Insert into Users (Username,Email,Password,ProfilePicture) values(@username, @email , @password, @avatar)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    if (avatarBytes != null)
                    {
                        command.Parameters.AddWithValue("@avatar", avatarBytes);
                    }
                    else
                    {
                        SqlParameter imageParameter = new SqlParameter("@avatar", SqlDbType.Image);
                        imageParameter.Value = DBNull.Value;
                        command.Parameters.Add(imageParameter);
                    }
                    command.ExecuteNonQuery();
                }
            }
            // clear error message
            ErrorMessageLabel.Content = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //To close the application
            Application.Current.Shutdown();
        }
        private void SelectAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                avatarFilePath = openFileDialog.FileName;
                AvatarImage.Source = new BitmapImage(new Uri(avatarFilePath));
            }
        }
    }
}
