using InTouch.Models;
using InTouch.Models.DbModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace InTouch.ViewModels
{
    public class ChattingVM : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context = new();
        private User _user = new();
        private List<User> _users = new();

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyPropertyChanged("User");
            }
        }

        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        public ChattingVM()
        {
            FillUsersList();
        }

        public void FillUsersList()
        {
            Users = _context.Users.ToList();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
