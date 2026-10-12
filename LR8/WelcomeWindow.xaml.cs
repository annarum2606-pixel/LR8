using System.Windows;
using static LR8.MainWindow;

namespace LR8
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();

            WelcomeLabel.Content = $"Добро пожаловать, {Users.CurrentUserLogin}, ваша роль - {Users.CurrentUserRole}.";
        }
    }
}
