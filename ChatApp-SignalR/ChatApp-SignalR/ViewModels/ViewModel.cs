using ChatApp_SignalR.Commands;
using ChatApp_SignalR.CustomControls;
using ChatApp_SignalR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp_SignalR.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Main Window 

        #region Property
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string LastSeen { get; set; }
        #endregion

        #endregion

        #region StatusThumbs
        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }

        public void LoadStatusThums()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                //Since we want to keep first status blank for the user to add own status
            new StatusDataModel
            {
                IsMeAddStatus=true
            },
            new StatusDataModel
            {
              ContactName="Mike",
              ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute),
              StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
              IsMeAddStatus=false
            },
            new StatusDataModel
            {
              ContactName="Steve",
              ContactPhoto=new Uri("/assets/download.jpg", UriKind.RelativeOrAbsolute),
              StatusImage=new Uri("/assets/8.jpg", UriKind.RelativeOrAbsolute),
              IsMeAddStatus=false
            },
            new StatusDataModel
            {
              ContactName="Will",
              ContactPhoto=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
              StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
              IsMeAddStatus=false
            },

            new StatusDataModel
            {
              ContactName="John",
              ContactPhoto=new Uri("/assets/4.jpg", UriKind.RelativeOrAbsolute),
              StatusImage=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
              IsMeAddStatus=false
            },
            };
            OnPropertyChanged("statusThumbsCollection");
        }
        #region Property
        #endregion
        #region Logic
        #endregion
        #endregion

        #region Chat List
        #region Property
        public ObservableCollection<ChatListData> Chats { get; set; }
        #endregion

        #region Logics
        public void LoadChats()
        {
            Chats = new ObservableCollection<ChatListData>()
            {
                new ChatListData
                {
                    ContactName = "Charlie",
                    ContactPhoto = new Uri("/assets/6.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                },
                new ChatListData
                {
                    ContactName = "Peter",
                    ContactPhoto = new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                },
                new ChatListData
                {
                    ContactName = "Elwyn",
                    ContactPhoto = new Uri("/assets/4.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                },
                new ChatListData
                {
                    ContactName = "Charlie",
                    ContactPhoto = new Uri("/assets/6.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                },
                new ChatListData
                {
                    ContactName = "Charlie",
                    ContactPhoto = new Uri("/assets/6.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                }
            };
            OnPropertyChanged();
        }
        #endregion

        #region Command
        //to get the ContactName of Selected chat so that we can open corresponding conversation
        protected ICommand _getSelectedCommand;
        public ICommand GetSelectedCommand => _getSelectedCommand ??= new RelayCommand(parameter =>
        {
            if(parameter is ChatListData v)
            {
                //getting contact name from selected chat
                ContactName = v.ContactName;
                OnPropertyChanged("ContactName");
                //getting contact photo from selected chat
                ContactPhoto = v.ContactPhoto;
                OnPropertyChanged("ContactPhoto");
            }
        });

        

        #endregion
        #endregion

        #region Conversations
        #region Property
        protected ObservableCollection<ChatConversations> _Conversations;
        public ObservableCollection<ChatConversations> Conversations
        {
            get => _Conversations;
            set
            {
                _Conversations = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Logics
        public void LoadChatConversations()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if (Conversations == null)
                Conversations = new ObservableCollection<ChatConversations>();
            using (SqlCommand command = new SqlCommand("select * from conversations where ContactName = 'Mike'", connection))
            {
                using(SqlDataReader reader = command.ExecuteReader()){
                    while (reader.Read())
                    {
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MM dd,hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MM dd,hh:mm tt") : "";
                        var conversation = new ChatConversations()
                        {
                            ContactName = reader["ContactName"].ToString(),
                            ReceivedMessage = reader["ReceivedMsgs"].ToString(),
                            MsgReceivedOn = MsgReceivedOn,
                            SentMessage = reader["SentMsgs"].ToString(),
                            MsgSentOn = MsgSentOn,
                            IsMessageReceived = string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? false : true
                        };
                        Conversations.Add(conversation);
                        OnPropertyChanged("Conversations");
                    }
                }
            }
        }
        SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\charlie.nguyen\\Documents\\LeaningSpace\\WPFLearning\\ChatApp-SignalR\\ChatApp-SignalR\\ChatApp-SignalR\\Database\\Database1.mdf;Integrated Security=True");
        #endregion
        #endregion
        public ViewModel()
        {
            LoadStatusThums();
            LoadChats();
            LoadChatConversations();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
