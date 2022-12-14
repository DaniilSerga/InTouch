using InTouch.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly LogVM _vm = new();

        public LogPage(MainWindow mainWindow)
        {
            InitializeComponent();

            DataContext = _vm;

            _mainWindow = mainWindow;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.MainWindow.Close();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });

            e.Handled = true;
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.User.Password = PasswordBox.Password;

            try
            {
                if (_vm.IsUserExists())
                {
                    _mainWindow.StartFrame.Navigate(new MainMenu(_vm));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void RegistrationLink_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartFrame.Navigate(new RegPage(_mainWindow));
        }
    }
}
