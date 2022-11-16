using InTouch.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
            
            _vm.User = regVM.User;

            DataContext = _vm;

            FillFrames();
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void FillFrames()
        {
            ChatFrame.Navigate(new ChattingPage(_vm));
            ProfileFrame.Navigate(new UserProfilePage(_vm));
            AdminPanelFrame.Navigate(new AdminPanel());
        }
    }
}
