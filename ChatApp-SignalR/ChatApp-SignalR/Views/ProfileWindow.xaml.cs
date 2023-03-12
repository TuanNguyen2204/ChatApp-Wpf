using ChatApp_SignalR.Models;
using ChatApp_SignalR.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChatApp_SignalR.Views
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private readonly Users _user;

        private HubConnection _connection;
        public ProfileWindow(Users user)
        {
            InitializeComponent();
            _user = user;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new ProfileViewModel()
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Avatar = user.ProfilePicture
            };
        }


        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                Title = "Select Avatar Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Load the selected image file
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();

                    // Convert the BitmapImage to a byte array
                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    var viewModel = DataContext as ProfileViewModel;
                    viewModel.Avatar = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image file: " + ex.Message);
                }
            }
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProfileViewModel;

            var userUpdate = new Users
            {
                UserId = _user.UserId,
                Username = viewModel.Username,
                Email = viewModel.Email,
                Password = viewModel.Password,
                ProfilePicture = viewModel.Avatar
            };

            try
            {
                bool update = UpdateUserProfile(userUpdate);
                if (update)
                {
                    MessageBox.Show("Profile updated successfully!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow(userUpdate);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to update profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Discard changes
            MainWindow mainWindow = new MainWindow(_user);
            mainWindow.Show();
            this.Close();
        }

        private bool UpdateUserProfile(Users user)
        {
            // Connect to the database
            string connectionString = App.GetConnectString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Create an SQL query to update the user profile
                string sql = "UPDATE Users SET Username=@username, Email=@email, Password=@password, ProfilePicture=@avatar WHERE UserId=@userid";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@avatar", user.ProfilePicture);
                    cmd.Parameters.AddWithValue("@userid", user.UserId);
                    int i = cmd.ExecuteNonQuery();
                    return i > 0; // Return true if at least one row was affected
                }
            }
        }

    }
}
