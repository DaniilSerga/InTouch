using InTouch.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private MainMenuVM _vm = new();

        #region Constructors
        public MainMenu()
        {
            InitializeComponent();

            FillFrames();
        }

        public MainMenu(LogVM logVM)
        {
            InitializeComponent();

            _vm.User = logVM.User;

            DataContext = _vm;

            FillFrames();
        }

        public MainMenu(RegVM regVM)
        {
            InitializeComponent();

            FillFrames();

            _vm.User = regVM.User;
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void FillFrames()
        {
            ChatFrame.Navigate(new ChattingPage(_vm));
            ProfileFrame.Navigate(new UserProfilePage(_vm));
        }
    }
}
