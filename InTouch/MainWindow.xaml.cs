using InTouch.Models.DbModels;
using InTouch.Views;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Windows;
using System.Windows.Input;

namespace InTouch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StartFrame.Navigate(new RegPage(this));
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
