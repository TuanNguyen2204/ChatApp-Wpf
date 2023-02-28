using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp_SignalR.Models
{
    public class ChatListData : INotifyPropertyChanged
    {
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        protected string message { get; set; }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
        protected string lastMessageTime { get; set; }
        public string LastMessageSentTime
        {
            get
            {
                return lastMessageTime;
            }
            set
            {
                lastMessageTime = value;
                OnPropertyChanged("LastMessageSentTime");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}