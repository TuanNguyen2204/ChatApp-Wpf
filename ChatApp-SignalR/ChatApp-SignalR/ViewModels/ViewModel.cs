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

        public ViewModel()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                //Since we want to keep first status blank for the user to add own status
                new StatusDataModel
                {
                    IsMeAddStatus = true
                },
                new StatusDataModel
                {
                    ContactName = "Charlie",
                    ContactPhoto = new Uri("/assets/1.png"),
                    StatusImage = new Uri("/assets/5.png"),
                    IsMeAddStatus = false
                }, new StatusDataModel
                {
                    ContactName = "Matthew",
                    ContactPhoto = new Uri("/assets/2.png"),
                    StatusImage = new Uri("/assets/5.png"),
                    IsMeAddStatus = false
                }, new StatusDataModel
                {
                    ContactName = "Elwyn",
                    ContactPhoto = new Uri("/assets/3.png"),
                    StatusImage = new Uri("/assets/5.png"),
                    IsMeAddStatus = false
                },
            };
            OnPropertyChanged("statusThumbsCollection");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
