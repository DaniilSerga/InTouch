using InTouch.Models;
using InTouch.Models.DbModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace InTouch.ViewModels
{
    public class LogVM : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context = new();
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

        public LogVM()
        {

        }

        public bool IsUserExists()
        {
            bool isExists = true;

            try
            {
                isExists = _context.Users.Any(u => u.Email == _user.Email && u.Password == _user.Password);

                _user = _context.Users.FirstOrDefault(u => u.Email == _user.Email && u.Password == _user.Password)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return false;
            }

            return isExists;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
