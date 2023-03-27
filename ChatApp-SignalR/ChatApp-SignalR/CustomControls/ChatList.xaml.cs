using ChatApp_SignalR.Models;
using ChatApp_SignalR.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp_SignalR.CustomControls
{
    /// <summary>
    /// Interaction logic for ChatList.xaml
    /// </summary>
    public partial class ChatList : UserControl
    {
        public ChatList()
        {
            InitializeComponent();
        }

        private void DeleteContact_Click(object sender, RoutedEventArgs e)
        {
            var chat = (sender as FrameworkElement).DataContext as ChatListData;
            string connectionString = App.GetConnectString();
            string query  = "DELETE FROM Conversations WHERE receive_id = @receive_id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a new SqlCommand object with the SQL query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the contactId parameter to the command
                    command.Parameters.AddWithValue("@receive_id", chat.Id);

                    // Execute the SQL command to delete the contact
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected by the command
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Contact not found.");
                    }
                }
            }
        }
    }
}
