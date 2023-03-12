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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatApp_SignalR.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        //Initializing resource dictionary file
        private readonly ResourceDictionary dictionary = Application.LoadComponent(new Uri("/ChatApp-SignalR;component/Assets/icons.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
        #region Main Window 

        #region Property
        public string ContactName { get; set; }
        public byte[] ContactPhoto { get; set; }
        public string LastSeen { get; set; }

        #region Search Chats

        protected bool _isSearchBoxOpen;
        public bool IsSearchBoxOpen
        {
            get => _isSearchBoxOpen;
            set
            {
                if (_isSearchBoxOpen == value)
                    return;

                _isSearchBoxOpen = value;


                if (_isSearchBoxOpen == false)
                    //Clear Search Box
                    SearchText = string.Empty;
                OnPropertyChanged("IsSearchBoxOpen");
                OnPropertyChanged("SearchText");
            }
        }

        //This is our list containing the Window Options..
        private ObservableCollection<MoreOptionsMenu> _windowMoreOptionsMenuList;
        public ObservableCollection<MoreOptionsMenu> WindowMoreOptionsMenuList
        {
            get
            {
                return _windowMoreOptionsMenuList;
            }
            set
            {
                _windowMoreOptionsMenuList = value;
            }
        }
        protected string LastSearchText { get; set; }
        protected string mSearchText { get; set; }
        public string SearchText
        {
            get => mSearchText;
            set
            {

                //checked if value is different
                if (mSearchText == value)
                    return;

                //Update Value
                mSearchText = value;

                //if search text is empty restore messages
                if (string.IsNullOrEmpty(SearchText))
                    Search();
            }
        }

        public void OpenSearchBox()
        {
            IsSearchBoxOpen = true;
        }
        #endregion
        #endregion

        #region Logics
        public void Search()

        {
            //To avoid re searching same text again
            if ((string.IsNullOrEmpty(LastSearchText) && string.IsNullOrEmpty(SearchText)) || string.Equals(LastSearchText, SearchText))
                return;

            //If searchbox is empty or chats is null pr chat cound less than 0
            if (string.IsNullOrEmpty(SearchText) || Chats == null || Chats.Count <= 0)
            {
                FilteredChats = new ObservableCollection<ChatListData>(Chats ?? Enumerable.Empty<ChatListData>());
                OnPropertyChanged("FilteredChats");
                //Update Last search Text
                LastSearchText = SearchText;

                return;
            }

            //Now, to find all chats that contain the text in our search box

            //if that chat is in Normal Unpinned Chat list find there...


            FilteredChats = new ObservableCollection<ChatListData>(
                Chats.Where(
                    chat => chat.ContactName.ToLower().Contains(SearchText) //if ContactName Contains SearchText then add it in filtered chat list
                    ||
                    chat.Message != null && chat.Message.ToLower().Contains(SearchText) //if Message Contains SearchText then add it in filtered chat list
                    ));
            OnPropertyChanged(nameof(FilteredChats));

            //Update Last search Text
            LastSearchText = SearchText;
        }

        public void ClearSearchBox()
        {
            if (!string.IsNullOrEmpty(SearchText))
                SearchText = string.Empty;
            else
                CloseSearchBox();
        }

        public void CloseSearchBox() => IsSearchBoxOpen = false;

        #region Window: More options popup
        void WindowMoreOptionsMenu()
        {
            WindowMoreOptionsMenuList = new ObservableCollection<MoreOptionsMenu>()
            {
                new MoreOptionsMenu()
                {
                 Icon = (PathGeometry)dictionary["newgroup"],
                 MenuText="New Group"
                },
                new MoreOptionsMenu()
                {
                 Icon = (PathGeometry)dictionary["settings"],
                 MenuText="Settings"
                },
            };
            OnPropertyChanged("WindowMoreOptionsMenuList");
        }
        void ConversationScreenMoreOptionsMenu()
        {
            //To populate menu items for conversation screen options list..
            WindowMoreOptionsMenuList = new ObservableCollection<MoreOptionsMenu>()
            {
                new MoreOptionsMenu()
                {
                 Icon = (PathGeometry)dictionary["clearchat"],
                 MenuText="Clear Chat"
                }
            };
            OnPropertyChanged("WindowMoreOptionsMenuList");
        }
        #endregion
        #endregion

        #region Commands
        protected ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new CommandViewModel(Search);
                return _searchCommand;
            }
            set
            {
                _searchCommand = value;
            }
        }

        /// <summary>
        /// Clear Search Command
        /// </summary>
        protected ICommand _clearSearchCommand;
        public ICommand ClearSearchCommand
        {
            get
            {
                if (_clearSearchCommand == null)
                    _clearSearchCommand = new CommandViewModel(ClearSearchBox);
                return _clearSearchCommand;
            }
            set
            {
                _clearSearchCommand = value;
            }
        }


        /// <summary>
        /// Search Command
        /// </summary>
        protected ICommand _openSearchCommand;
        public ICommand OpenSearchCommand
        {
            get
            {
                if (_openSearchCommand == null)
                    _openSearchCommand = new CommandViewModel(OpenSearchBox);
                return _openSearchCommand;
            }
            set
            {
                _openSearchCommand = value;
            }
        }

        protected ICommand _windowsMoreOptionsCommand;
        public ICommand WindowsMoreOptionsCommand
        {
            get
            {
                if (_windowsMoreOptionsCommand == null)
                    _windowsMoreOptionsCommand = new CommandViewModel(WindowMoreOptionsMenu);
                return _windowsMoreOptionsCommand;
            }
            set
            {
                _windowsMoreOptionsCommand = value;
            }
        }

        protected ICommand _conversationMoreOptionsCommand;
        public ICommand ConversationMoreOptionsCommand
        {
            get
            {
                if (_conversationMoreOptionsCommand == null)
                    _conversationMoreOptionsCommand = new CommandViewModel(ConversationScreenMoreOptionsMenu);
                return _conversationMoreOptionsCommand;
            }
            set
            {
                _conversationMoreOptionsCommand = value;
            }
        }
        #endregion



        #endregion

        #region StatusThumbs
        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }

        #region Property
        #endregion
        #region Logic
        #endregion
        #endregion

        #region Chat List
        #region Property
        public ObservableCollection<ChatListData> mChats;
        public ObservableCollection<ChatListData> Chats
        {
            get => mChats;
            set
            {
                //To Change the list
                if (mChats == value)
                    return;

                //To Update the list
                mChats = value;

                //Updating filtered chats to match
                FilteredChats = new ObservableCollection<ChatListData>(mChats);
                OnPropertyChanged("Chats");
                OnPropertyChanged("FilteredChats");
            }
        }

        //Filtering Chats
        //private ObservableCollection<ChatListData> _filteredChats;
        //public ObservableCollection<ChatListData> FilteredChats
        //{
        //    get { return _filteredChats; }
        //    set
        //    {
        //        if (_filteredChats != value)
        //        {
        //            _filteredChats = value;
        //            OnPropertyChanged("FilteredChats");
        //        }
        //    }
        //}
        public ObservableCollection<ChatListData> FilteredChats { get; set; }
        #endregion

        #region Logics
        public void LoadChats(Users user)
        {
            
            //Loading chats data from db
            if (Chats == null)
                Chats = new ObservableCollection<ChatListData>();

            //Openning Sql connection
            connection.Open();
            //Temporary collection
            ObservableCollection<ChatListData> temp = new ObservableCollection<ChatListData>();
            using (SqlCommand command = new SqlCommand("select * from (select * from (select UserID from dbo.Users p where p.UserId = @userid) d left join (select a.*, row_number() over(partition by a.sender_id order by a.id desc) as seqnum from Conversations a ) a on a.sender_id = d.UserID and a.seqnum = 1) q join Users u on q.receive_id = u.UserId order by q.Id desc", connection))
            {
                command.Parameters.AddWithValue("@userid", user.UserId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //To avoid duplication
                    string lastItem = string.Empty;
                    string newItem = string.Empty;
                    while (reader.Read()) {
                        string time = string.Empty;
                        string lastMessage = string.Empty;
                        if (!string.IsNullOrEmpty(reader["msgReceivedOn"].ToString()))
                        {
                            time = Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("ddd hh:mm tt");
                            lastMessage = reader["ReceivedMsgs"].ToString();
                        }
                        //else if we have sent last message then update accordingly...
                        if (!string.IsNullOrEmpty(reader["MsgSentOn"].ToString()))
                        {
                            time = Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("ddd hh:mm tt");
                            lastMessage = reader["SentMsgs"].ToString();
                        }
                        //if the chat is new or we are starting new conversation which means there will be no previous sent or received msgs in that case..
                        //show 'Start new conversation' message...
                        if (string.IsNullOrEmpty(lastMessage))
                            lastMessage = "Start new conversation";

                        //Update data in model...
                        var contactid = reader["receive_id"].ToString();
                        SqlCommand command1 = new SqlCommand("Select * from Users where UserID = @contactid", connection);
                        command1.Parameters.AddWithValue("@contactid", contactid);
                        string contactname = reader["Username"].ToString();
                        byte[] profilePicture = (byte[])reader["ProfilePicture"];
                        ChatListData chat = new ChatListData()
                        {
                            Id = Int32.Parse(contactid),
                            ContactPhoto = profilePicture,
                            ContactName = contactname,
                            Message = lastMessage,
                            LastMessageSentTime = time
                        };

                        //Update 
                        newItem = contactname;

                        //If last added chat contact is not same as new one then only add
                        if (lastItem != newItem)
                            temp.Add(chat);
                        lastItem = newItem;
                    }
                }
            }

            Chats = temp;
                //Update
                OnPropertyChanged("Chats");
            OnPropertyChanged();

        }
        #endregion

        #region Command
        //to get the ContactName of Selected chat so that we can open corresponding conversation
        protected ICommand _getSelectedCommand;
        public ICommand GetSelectedCommand => _getSelectedCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ChatListData v)
            {
                //getting contact name from selected chat
                ContactName = v.ContactName;
                OnPropertyChanged("ContactName");
                //getting contact photo from selected chat
                ContactPhoto = v.ContactPhoto;
                OnPropertyChanged("ContactPhoto");

                LoadChatConversations(v);
            }
        });



        #endregion
        #endregion

        #region Conversations
        #region Property
        protected ObservableCollection<ChatConversations> mConversations;
        public ObservableCollection<ChatConversations> Conversations
        {
            get => mConversations;
            set
            {
                //To Change the list
                if (mConversations == value)
                    return;

                //To Update the list
                mConversations = value;

                //Updating filtered chats to match
                FilteredConversations = new ObservableCollection<ChatConversations>(mConversations);
                OnPropertyChanged("Conversations");
                OnPropertyChanged("FilteredConversations");
            }
        }

        /// <summary>
        /// Filter Conversation
        /// </summary>
        public ObservableCollection<ChatConversations> FilteredConversations { get; set; }

        //We will use this message text to transfer the send message value to our conversation body
        protected string messageText;
        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }
        protected string LastSearchConversationText;
        protected string mSearchConversationText;
        public string SearchConversationText
        {
            get => mSearchConversationText;
            set
            {

                //checked if value is different
                if (mSearchConversationText == value)
                    return;

                //Update Value
                mSearchConversationText = value;

                //if search text is empty restore messages
                if (string.IsNullOrEmpty(SearchConversationText))
                    SearchInConversation();
            }
        }
        
        public bool FocusMessageBox { get; set; }
        public bool IsThisAReplyMessage { get; set; }
        public string MessageToReplyText { get; set; }

        protected bool _isSearchConversationBoxOpen;
        public bool IsSearchConversationBoxOpen
        {
            get => _isSearchConversationBoxOpen;
            set
            {
                if (_isSearchConversationBoxOpen == value)
                    return;

                _isSearchConversationBoxOpen = value;


                if (_isSearchConversationBoxOpen == false)
                    //Clear Search Box
                    SearchConversationText = string.Empty;
                OnPropertyChanged("IsSearchConversationBoxOpen");
                OnPropertyChanged("SearchConversationText");
            }
        }
        #endregion
        #region Command

        protected ICommand _searchConversationCommand;
        public ICommand SearchConversationCommand
        {
            get
            {
                if (_searchConversationCommand == null)
                    _searchConversationCommand = new CommandViewModel(SearchInConversation);
                return _searchConversationCommand;
            }
            set
            {
                _searchConversationCommand = value;
            }
        }


        /// <summary>
        /// Reply Command
        /// </summary>
        protected ICommand _replyCommand;
        public ICommand ReplyCommand => _replyCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ChatConversations v)
            {
                //if replying sender's message
                if (v.IsMessageReceived)
                    MessageToReplyText = v.ReceivedMessage;
                //if replying own message
                else
                    MessageToReplyText = v.SentMessage;

                //update
                OnPropertyChanged("MessageToReplyText");

                //Set focus on Message Box when user clicks reply button
                FocusMessageBox = true;
                OnPropertyChanged("FocusMessageBox");

                //Flag this message as reply message
                IsThisAReplyMessage = true;
                OnPropertyChanged("IsThisAReplyMessage");
            }
        });

        protected ICommand _cancelReplyCommand;
        public ICommand CancelReplyCommand
        {
            get
            {
                if (_cancelReplyCommand == null)
                    _cancelReplyCommand = new CommandViewModel(CancelReply);
                return _cancelReplyCommand;
            }
            set
            {
                _cancelReplyCommand = value;
            }
        }

        protected ICommand _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (_sendMessageCommand == null)
                    _sendMessageCommand = new CommandViewModel(SendMessage);
                return _sendMessageCommand;
            }
            set
            {
                _sendMessageCommand = value;
            }
        }

        /// <summary>
        /// Search Command
        /// </summary>
        protected ICommand _openConversationSearchCommand;
        public ICommand OpenConversationSearchCommand
        {
            get
            {
                if (_openConversationSearchCommand == null)
                    _openConversationSearchCommand = new CommandViewModel(OpenConversationSearchBox);
                return _openConversationSearchCommand;
            }
            set
            {
                _openConversationSearchCommand = value;
            }
        }

        /// <summary>
        /// Clear Search Command
        /// </summary>
        protected ICommand _clearConversationSearchCommand;
        public ICommand ClearConversationSearchCommand
        {
            get
            {
                if (_clearConversationSearchCommand == null)
                    _clearConversationSearchCommand = new CommandViewModel(ClearConversationSearchBox);
                return _clearConversationSearchCommand;
            }
            set
            {
                _clearConversationSearchCommand = value;
            }
        }
        #endregion

        #region Logics

        public void OpenConversationSearchBox()
        {
            IsSearchConversationBoxOpen = true;
        }

        public void ClearConversationSearchBox()
        {
            if (!string.IsNullOrEmpty(SearchConversationText))
                SearchConversationText = string.Empty;
            else
                CloseConversationSearchBox();
        }

        public void CloseConversationSearchBox() => IsSearchConversationBoxOpen = false;
        public void LoadChatConversations(ChatListData chat)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if (Conversations == null)
                Conversations = new ObservableCollection<ChatConversations>();
            Conversations.Clear();
            FilteredConversations.Clear();
            using (SqlCommand command = new SqlCommand("select * from Conversations where receive_id=@contactid", connection))
            {
                command.Parameters.AddWithValue("@contactid", chat.Id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MM dd,hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MM dd,hh:mm tt") : "";
                        var conversation = new ChatConversations()
                        {
                            ContactName = chat.ContactName,
                            ReceivedMessage = reader["ReceivedMsgs"].ToString(),
                            MsgReceivedOn = MsgReceivedOn,
                            SentMessage = reader["SentMsgs"].ToString(),
                            MsgSentOn = MsgSentOn,
                            IsMessageReceived = string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? false : true,
                            MessageToReplyText = MessageToReplyText
                        };
                        Conversations.Add(conversation);
                        OnPropertyChanged("Conversations");
                        FilteredConversations.Add(conversation);
                        OnPropertyChanged("FilteredConversations");

                        chat.Message = !string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? reader["ReceivedMsgs"].ToString() : reader["SentMsgs"].ToString();
                    }
                }
            }
            //Reset reply message text when the new chat is fetched
            MessageToReplyText = string.Empty;
            OnPropertyChanged("MessageToReplyText");
        }

        void SearchInConversation()
        {
            //To avoid re searching same text again
            if ((string.IsNullOrEmpty(LastSearchConversationText) && string.IsNullOrEmpty(SearchConversationText)) || string.Equals(LastSearchConversationText, SearchConversationText))
                return;

            //If searchbox is empty or Conversations is null pr chat cound less than 0
            if (string.IsNullOrEmpty(SearchConversationText) || Conversations == null || Conversations.Count <= 0)
            {
                FilteredConversations = new ObservableCollection<ChatConversations>(Conversations ?? Enumerable.Empty<ChatConversations>());
                OnPropertyChanged("FilteredConversations");

                //Update Last search Text
                LastSearchConversationText = SearchConversationText;

                return;
            }

            //Now, to find all Conversations that contain the text in our search box

            FilteredConversations = new ObservableCollection<ChatConversations>(
                Conversations.Where(chat => chat.ReceivedMessage.ToLower().Contains(SearchConversationText) || chat.SentMessage.ToLower().Contains(SearchConversationText)));
            OnPropertyChanged("FilteredConversations");

            //Update Last search Text
            LastSearchConversationText = SearchConversationText;
        }

        public void CancelReply()
        {
            IsThisAReplyMessage = false;
            //Reset Reply Message Text
            MessageToReplyText = string.Empty;
            OnPropertyChanged("MessageToReplyText");
        }

        public void SendMessage()
        {
            //Send Message only when the textbox is not empty
            if (!string.IsNullOrEmpty(MessageText))
            {
                var conversation = new ChatConversations()
                {
                    ReceivedMessage = MessageToReplyText,
                    SentMessage = MessageText,
                    MsgSentOn = DateTime.Now.ToString("MMM dd, hh:mm tt"),
                    MessageContainsReply = IsThisAReplyMessage,
                    MessageToReplyText = MessageToReplyText
                };
                //Add message to converstion list
                FilteredConversations.Add(conversation);
                Conversations.Add(conversation);

                UpdateChatAndMoveUp(Chats, conversation);
                UpdateChatAndMoveUp(FilteredChats, conversation);

                //Clear Message properties and textbox when message is sent
                MessageText = string.Empty;
                IsThisAReplyMessage = false;
                MessageToReplyText = string.Empty;

                

                //Update
                OnPropertyChanged("FilteredConversations");
                OnPropertyChanged("Conversations");
                OnPropertyChanged("MessageText");
                OnPropertyChanged("IsThisAReplyMessage");
                OnPropertyChanged("MessageToReplyText");

            }
        }

        //Move the chat contact on top when new message is sent or received
        protected void UpdateChatAndMoveUp(ObservableCollection<ChatListData> chatList, ChatConversations conversation)
        {
            //Check if the message sent is to the selected contact or not...
            var chat = chatList.FirstOrDefault(x => x.ContactName == ContactName);

            //if found
            if (chat != null)
            {
                //Update Contact Chat Last Message and Message Time..
                chat.Message = MessageText;
                chat.LastMessageSentTime = conversation.MsgSentOn;

                //Move Chat on top when new message is received/sent...
                chatList.Move(chatList.IndexOf(chat), 0);

                //Update Collections
                OnPropertyChanged("Chats");
                OnPropertyChanged("FilteredChats");
            }
        }

        //SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\charlie.nguyen\\Documents\\LeaningSpace\\WPFLearning\\ChatApp-SignalR\\ChatApp-SignalR\\ChatApp-SignalR\\Database\\Database1.mdf;Integrated Security=True");
        SqlConnection connection = new SqlConnection(App.GetConnectString());
        #endregion
        #endregion

        #region ContactInfo
        #region Property
        protected bool _IsContactInfoOpen;
        public bool IsContactInfoOpen
        {
            get => _IsContactInfoOpen;
            set
            {
                _IsContactInfoOpen = value;
                OnPropertyChanged("IsContactInfoOpen");
            }
        }
        #endregion
        #region Logics
        public void OpenContactInfo() => IsContactInfoOpen = true;
        public void CloseContactInfo() => IsContactInfoOpen = false;
        #endregion

        #region Command
        /// <summary>
        /// Open ContactInfo Command
        /// </summary>
        protected ICommand _openContactInfoCommand;
        public ICommand OpenContactinfoCommand
        {
            get
            {
                if (_openContactInfoCommand == null)
                    _openContactInfoCommand = new CommandViewModel(OpenContactInfo);
                return _openContactInfoCommand;
            }
            set
            {
                _openContactInfoCommand = value;
            }
        }

        /// <summary>
        /// Open ContactInfo Command
        /// </summary>
        protected ICommand _closeontactInfoCommand;
        public ICommand CloseContactinfoCommand
        {
            get
            {
                if (_closeontactInfoCommand == null)
                    _closeontactInfoCommand = new CommandViewModel(CloseContactInfo);
                return _closeontactInfoCommand;
            }
            set
            {
                _closeontactInfoCommand = value;
            }
        }
        #endregion
        #endregion
        private Users user;
        public ViewModel(Users user)
        {
            LoadChats(user);
            this.user = user;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
