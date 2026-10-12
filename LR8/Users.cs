namespace LR8
{
public partial class MainWindow
    {
        public class Users
        {
            public static string CurrentUserLogin = "";
            public static string CurrentUserRole = "";

            public static (string Login, string Password, string Role)[] users =
            {
                ("manager", "1", "менеджер"),
                ("admin", "2", "администратор"),
                ("user", "3", "пользователь")
            };
        }
    }
}