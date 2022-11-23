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

        public bool IsUserExists()
        {
            if (_user is null)
            {
                throw new Exception("Введите данные");
            }

            if (string.IsNullOrEmpty(_user.Email))
            {
                throw new Exception("Укажите Email");
            }

            if (string.IsNullOrEmpty(_user.Password))
            {
                throw new Exception("Укажите пароль");
            }

            bool isExists = true;

            try
            {
                isExists = _context.Users.Any(u => u.Email == _user.Email && u.Password == _user.Password);

                if (isExists)
                {
                    _user = _context.Users.FirstOrDefault(u => u.Email == _user.Email && u.Password == _user.Password)!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (!isExists)
            {
                throw new Exception("Ошибка авторизации (проверьте введённые данные)");
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
