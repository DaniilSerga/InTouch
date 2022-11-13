using InTouch.Models.DbModels;
using System.ComponentModel;

namespace InTouch.ViewModels
{
    public class MainMenuVM : INotifyPropertyChanged
    {
        private User _user = new();

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyPropertyChanged("User");
            }
        }

        public MainMenuVM()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
