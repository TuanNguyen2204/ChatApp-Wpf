using ChatApp_SignalR.CustomControls;
using ChatApp_SignalR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp_SignalR.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }


        #region StatusThumbs
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
                    ContactName = "Charlie",
                    ContactPhoto = new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    Message = "Hello, How are you?",
                    LastMessageSentTime = "Tue, 12:58 PM"
                },
                new ChatListData
                {
                    ContactName = "Charlie",
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
