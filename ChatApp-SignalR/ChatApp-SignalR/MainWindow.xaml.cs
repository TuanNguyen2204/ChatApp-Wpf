using ChatApp_SignalR.Models;
using ChatApp_SignalR.ViewModels;
using ChatApp_SignalR.Views;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp_SignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Users _user;
        public MainWindow(Users user)
        {
            InitializeComponent();
            _user = user;
            DataContext = new ViewModel(_user);
            var imageBytes = _user.ProfilePicture;
            BitmapImage imageSource = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                stream.Position = 0;
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.StreamSource = stream;
                imageSource.EndInit();
            }
            userImg.ImageSource = imageSource;
            username.Text = _user.Username;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {

            //To minimize the window
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //To close the application
            Application.Current.Shutdown();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profile = new ProfileWindow(_user);
            profile.Show();
            Hide();
        }
    }
}
