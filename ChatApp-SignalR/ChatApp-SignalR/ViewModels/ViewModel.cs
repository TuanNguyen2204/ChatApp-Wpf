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

        #endregion
        #endregion
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
        public void LoadChats()
        {
            Chats = new ObservableCollection<ChatListData>()
            {
                new ChatListData
                {
                    ContactName = "Mike",
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
            using (SqlCommand command = new SqlCommand("select * from conversations where ContactName=@ContactName", connection))
            {
                command.Parameters.AddWithValue("@ContactName", chat.ContactName);
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
                            ContactName = reader["ContactName"].ToString(),
                            ReceivedMessage = reader["ReceivedMsgs"].ToString(),
                            MsgReceivedOn = MsgReceivedOn,
                            SentMessage = reader["SentMsgs"].ToString(),
                            MsgSentOn = MsgSentOn,
                            IsMessageReceived = string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? false : true
                        };
                        Conversations.Add(conversation);
                        OnPropertyChanged("Conversations");
                        FilteredConversations.Add(conversation);
                        OnPropertyChanged("FilteredConversations");

                    }
                }
            }
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
        SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\charlie.nguyen\\Documents\\LeaningSpace\\WPFLearning\\ChatApp-SignalR\\ChatApp-SignalR\\ChatApp-SignalR\\Database\\Database1.mdf;Integrated Security=True");
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
        public ViewModel()
        {
            LoadStatusThums();
            LoadChats();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
