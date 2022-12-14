using InTouch.Models;
using InTouch.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace InTouch.ViewModels
{
    public class ChattingVM : INotifyPropertyChanged
    {
        private readonly ApplicationContext _context = new();
        private string? _output = string.Empty;
        private User _user = new();
        private List<User> _users = new();

        #region TCP configuration
        private TcpClient client = new TcpClient();
        private string host = "127.0.0.1";
        private int port = 8888;
        private StreamReader? Reader = null;
        private StreamWriter? Writer = null!;
        #endregion

        // This property is used for displaying everything what's sent and recieved
        public string? Output
        {
            get => string.Format(_output);
            set
            {
                _output = value;
                NotifyPropertyChanged("Output");
            }
        }

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

        public ChattingVM(User user)
        {
            _user = user;

            FillUsersList();

            ConnectUserToServer();
        }

        #region TCP behavior
        async void ConnectUserToServer()
        {
            try
            {
                // подключение клиента
                client.Connect(host, port);
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());

                if (Writer is null || Reader is null)
                {
                    return;
                }

                // запускаем новый поток для получения данных
                await Task.Run(() => ReceiveMessageAsync(Reader));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Writer?.Close();
            Reader?.Close();
        }

        // Starts listening and displays that user is online
        // Начало прослушивания и отображения остальным пользователям, что текущий пользователь теперь онлайн
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            // Sends user's full name to get welcoming message
            // Отправление ФИО пользователя, для того чтобы остальные пользователи увидели, кто подключился
            await Writer.WriteLineAsync($"{User.FullName} ({User.Position})");
            await Writer.FlushAsync();

            while (true)
            {
                try
                {
                    // Reads incoming message
                    // Чтение входящего сообщения
                    string? message = reader.ReadLine();

                    // Skip null or empty message
                    // В случае отправки пустого сообщения, отправка будет отменена
                    if (string.IsNullOrEmpty(message))
                    {
                        continue;
                    }

                    // Display message
                    // Отображение сообщения
                    Output += $"\n{message}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка К", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
        }

        // Sends message to a server
        public async Task SendMessageAsync(string message)
        {
            try
            {
                Output += $"\n{User.FullName} ({User.Position}): {message}";

                // Отправка сообщения
                await Writer.WriteLineAsync(message);
                await Writer.FlushAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        // Заполнение списка с пользователями на экране
        public void FillUsersList() => Users = _context.Users.Where(u => u.Email != User.Email).ToList();

        #region INotifyProperty section
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
