using InTouch.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for ChattingPage.xaml
    /// </summary>
    public partial class ChattingPage : Page
    {
        private ChattingVM _vm;

        public ChattingPage(MainMenuVM menuVM)
        {
            InitializeComponent();

            _vm = new(menuVM.User);

            DataContext = _vm;
        }

        private async void SendMessage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await _vm.SendMessageAsync(MessageBox.Text);

            MessageBox.Text = string.Empty;
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendMessage.RaiseEvent(new System.Windows.RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.FillUsersList();
        }
    }
}
