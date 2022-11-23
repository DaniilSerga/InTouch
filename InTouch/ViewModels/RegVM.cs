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
            #region input checks
            if (_user is null)
            {
                throw new Exception("Необходимые поля не были заполнены");
            }

            if (string.IsNullOrEmpty(_user.Email))
            {
                throw new Exception("Email не был указан");
            }

            if (!_user.Email!.Contains('@'))
            {
                throw new Exception("Адрес электронной почты был указан неверно.");
            }

            if (string.IsNullOrEmpty(_user.Position))
            {
                throw new Exception("Необходимо указать должность");
            }

            if (string.IsNullOrEmpty(_user.FullName))
            {
                throw new Exception("ФИО не было указано");
            }
            
            for (int i = 0; i < _user.FullName!.Length; i++)
            {
                if (char.IsNumber(_user.FullName[i]) || char.IsPunctuation(_user.FullName[i]))
                {
                    throw new Exception($"Вы ввели недопустимый символ {User.FullName![i]} в поле ФИО");
                }
            }
            #endregion

            if (_user.Password.Length <= 5)
            {
                throw new Exception("Минимальная длина пароля - 5 символов");
            }

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
                "\n\nДля доступа к чату вам необходимо перейти в него из соответствующей иконки в левой части приложения." +
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
            smtp.Authenticate("intouchcompany@rambler.ru", "r9i-nai-4Zw-rcE");

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
