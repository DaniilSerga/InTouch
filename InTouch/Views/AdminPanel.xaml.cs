using InTouch.Models.DbModels;
using InTouch.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace InTouch.Views
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        private readonly AdminPanelVM _vm = new();

        // TODO Complete design
        // It'll allow CEO to delete employees
        public AdminPanel()
        {
            InitializeComponent();

            DataContext = _vm;
        }

        private void DeleteUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UsersList.SelectedValue == null)
            {
                MessageBox.Show("Выберите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _vm.Delete((User)UsersList.SelectedValue);
        }
    }
}
