using InTouch.Views;
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

            // Displaying registration page
            StartFrame.Navigate(new RegPage(this));
        }

        // Allows to move the window by pressing and holding left mouse button
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
