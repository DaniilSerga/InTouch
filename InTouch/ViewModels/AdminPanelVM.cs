using InTouch.Models;
using InTouch.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace InTouch.ViewModels
{
    public class AdminPanelVM
    {
        private readonly ApplicationContext _context = new();
        private List<User> _users = new();

        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        public AdminPanelVM()
        {
            Users = _context.Users.Where(u => u.Position != "Директор").ToList();
        }

        public void Delete(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), "User was null while attempting to delete.");
            }

            try
            {
                if (_context.Users.Any(u => u == user))
                {
                    _context.Users.Remove(user);

                    _context.SaveChanges();

                    Users = _context.Users.Where(u => u.Position != "Директор").ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
