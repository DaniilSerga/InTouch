using InTouch.Models;
using InTouch.Models.DbModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InTouch.ViewModels
{
    public class RegVM : INotifyPropertyChanged
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

        public string Password { get; set; }

        public async Task RegistrateUser()
        {
            if (_context.Users.Any(u => u == _user))
            {
                throw new Exception("Данный пользователь уже зарегестриорван.");
            }

            if (_context.Users.Any(u => u.FullName == _user.FullName))
            {
                throw new Exception("Данное ФИО уже занято.");
            }

            if (_context.Users.Any(u => u.Email == _user.Email))
            {
                throw new Exception("Указаный E-mail уже занят.");
            }

            try
            {
                _context.Users.Add(_user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при регистрации.\n\n{ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
