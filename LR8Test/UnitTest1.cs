using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FluentAssertions;

namespace LR8Test
{
    public class LR8Test : IDisposable
    {
        private readonly Application _app;
        private readonly UIA3Automation _automation;
        private readonly Window _window;

        public LR8Test()
        {
            _app = Application.Launch(Path.Combine(Environment.CurrentDirectory,
                            @"..\..\..\..\LR8\bin\Debug\net10.0-windows\LR8.exe"));
            _automation = new UIA3Automation();
            _window = _app.GetMainWindow(_automation)!;
        }

        // Создание скриншота
        private void ScreenshotScreen(Window window, string name) =>
            window.Capture().Save($"{name}.png");

        [Fact]
        public void Authorization_EmptyAll_ShowsLoginError()
        {
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            var label = _window.FindFirstDescendant(cf => cf.ByAutomationId("ErrorLabel"))!.Name;

            Thread.Sleep(1000);
            ScreenshotScreen(_window, "Test1_EmptyAll");

            Assert.Equal("Заполните поле логин", label);
        }

        [Fact]
        public void Authorization_OnlyLogin_ShowsPasswordError()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("manager");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            var label = _window.FindFirstDescendant(cf => cf.ByAutomationId("ErrorLabel"))!.Name;

            Thread.Sleep(1000);
            ScreenshotScreen(_window, "Test2_OnlyLogin");

            Assert.Equal("Заполните поле пароль", label);
        }

        [Fact]
        public void Authorization_UnknownLogin_ShowsUserNotFound()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("555");
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("123");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            var label = _window.FindFirstDescendant(cf => cf.ByAutomationId("ErrorLabel"))!.Name;

            Thread.Sleep(1000);
            ScreenshotScreen(_window, "Test3_UnknownLogin");

            Assert.Equal("Пользователь не найден", label);
        }

        [Fact]
        public void Authorization_WrongPassword_ShowsError()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("manager");
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("7");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            var label = _window.FindFirstDescendant(cf => cf.ByAutomationId("ErrorLabel"))!.Name;

            Thread.Sleep(1000);
            ScreenshotScreen(_window, "Test4_WrongPassword");

            Assert.Equal("Неверный пароль", label);
        }

        [Fact]
        public void Authorization_SuccessLogin_Manager()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("manager");
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("1");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            Thread.Sleep(1000);

            var welcome = _app.GetAllTopLevelWindows(_automation)
                              .FirstOrDefault(w => w.Name == "WelcomeWindow");
            welcome.Should().NotBeNull();

            var label = welcome!
                .FindFirstDescendant(cf => cf.ByAutomationId("WelcomeLabel"))!.Name;

            Assert.Equal("Добро пожаловать, manager, ваша роль - менеджер.", label);

            ScreenshotScreen(welcome, "Test5_SuccessManager");
            welcome.Close();
        }

        [Fact]
        public void Authorization_OnlyPassword_ShowsLoginError()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("123");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            var label = _window.FindFirstDescendant(cf => cf.ByAutomationId("ErrorLabel"))!.Name;

            Thread.Sleep(1000);
            ScreenshotScreen(_window, "Test6_OnlyPassword");

            Assert.Equal("Заполните поле логин", label);
        }

        [Fact]
        public void Authorization_SuccessLogin_Admin()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("admin");
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("2");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            Thread.Sleep(1000);

            var welcome = _app.GetAllTopLevelWindows(_automation)
                              .FirstOrDefault(w => w.Name == "WelcomeWindow");

            welcome.Should().NotBeNull();

            var label = welcome!
                .FindFirstDescendant(cf => cf.ByAutomationId("WelcomeLabel"))!.Name;

            Assert.Equal("Добро пожаловать, admin, ваша роль - администратор.", label);

            ScreenshotScreen(welcome, "Test7_SuccessAdmin");
            welcome.Close();
        }

        public void Dispose()
        {
            _automation.Dispose();
            _app.Close();
            _app.Dispose();
        }
    }
}