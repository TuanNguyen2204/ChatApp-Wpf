using Microsoft.AspNetCore.SignalR;
using ChatServer.Models;
using Microsoft.Data.SqlClient;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _connectionString;
        public ChatHub(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task UpdateProfile(Users user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Username=@username, Email=@email, Password=@password, ProfilePicture=@avatar WHERE UserId=@userid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@avatar", user.ProfilePicture);
                command.Parameters.AddWithValue("@userid", user.UserId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            await Clients.All.SendAsync("UpdateUserProfile", user);
        }
   }
}