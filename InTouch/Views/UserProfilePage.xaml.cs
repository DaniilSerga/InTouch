using InTouch.ViewModels;
using System.Windows.Controls;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : Page
    {
        private readonly ProfileVM _vm = new();

        public UserProfilePage(MainMenuVM menuVM)
        {
            InitializeComponent();

            _vm.User = menuVM.User;

            DataContext = _vm;
        }
    }
}
