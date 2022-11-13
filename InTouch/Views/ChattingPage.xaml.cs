using InTouch.ViewModels;
using System.Windows.Controls;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for ChattingPage.xaml
    /// </summary>
    public partial class ChattingPage : Page
    {
        private ChattingVM _vm = new();

        public ChattingPage(MainMenuVM menuVM)
        {
            InitializeComponent();

            _vm.User = menuVM.User;

            DataContext = _vm;
        }
    }
}
