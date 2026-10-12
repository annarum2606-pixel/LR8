using System.Windows;

namespace LR8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AuthorizarionButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            var user = Users.users.FirstOrDefault(u => u.Login == login);

            //Проверка на пустой логин. Тест 1
            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Заполните поле логин");
                return;
            }

            //Проверка на пустой пароль. Тест 2
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните поле пароль");
                return;
            }

            //Проверка существования логина. Тест 3
            if (user == default)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            //Проверка верности пароля. Тест 4
            if (user.Password != password)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }

            //Успешная авторизация. Тест 5 (передача логина и роли)
            Users.CurrentUserLogin = login;
            Users.CurrentUserRole = user.Role;

            WelcomeWindow welcomeWindow = new();
            welcomeWindow.Show();
        }
    }
}