using InTouch.Models;
using InTouch.Models.DbModels;
using MailKit.Security;
using MimeKit;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

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

                SendWelcomingEmail();
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при регистрации.\n\n{ex.Message}");
            }
        }

        private async void SendWelcomingEmail()
        {
            BodyBuilder bodyBuilder = new()
            {
                TextBody = $"{User.FullName}, добро пожаловать в сеть для общения сотрудников компании InTouch!" +
                "\nДля доступа к чату вам необходимо перейти в него из соответствующей иконки в левой части приложения." +
                "\nТакже вы можете посмотреть информацию, которую вы указали при регистрации в своём профиле. Для доступа к нему так же обратитесь к левой части приложения." +
                "\n\nПриятного использования, ваша команда разработки InTouch <3"
            };

            var msg = new MimeMessage()
            {
                // Theme
                Subject = $"Добро пожаловать",
                // Appeal body
                Body = bodyBuilder.ToMessageBody()
            };

            msg.From.Add(MailboxAddress.Parse("intouchcompany@rambler.ru"));
            msg.To.Add(MailboxAddress.Parse(User.Email));

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.rambler.ru", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("intouchcompany@rambler.ru", "BRM-qF2-2H7-LNh");

            await smtp.SendAsync(msg);
            await smtp.DisconnectAsync(true);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
