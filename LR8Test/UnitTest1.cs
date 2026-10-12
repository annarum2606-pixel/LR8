using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
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

        private void ScreenshotScreen(string name) =>
            Capture.Screen().ToFile($"{name}.png");

        private void ScreenshotWindow(Window w, string name) =>
            Capture.Element(w).ToFile($"{name}.png");

        [Fact]
        public void Authorization_EmptyAll_ShowsLoginError()
        {
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            Thread.Sleep(1500);
            ScreenshotScreen("Test1_EmptyAll");

            Assert.True(File.Exists("Test1_EmptyAll.png"));
        }

        [Fact]
        public void Authorization_OnlyLogin_ShowsPasswordError()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("LoginTextBox"))!
                   .AsTextBox().Enter("manager");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            Thread.Sleep(1500);
            ScreenshotScreen("Test2_OnlyLogin");

            Assert.True(File.Exists("Test2_OnlyLogin.png"));
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

            Thread.Sleep(1500);
            ScreenshotScreen("Test3_UnknownLogin");

            Assert.True(File.Exists("Test3_UnknownLogin.png"));
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

            Thread.Sleep(1500);
            ScreenshotScreen("Test4_WrongPassword");

            Assert.True(File.Exists("Test4_WrongPassword.png"));
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

            Thread.Sleep(1500);

            var welcome = _app.GetAllTopLevelWindows(_automation)
                              .FirstOrDefault(w => w.Name == "WelcomeWindow");

            welcome.Should().NotBeNull();

            var label = welcome!
                .FindFirstDescendant(cf => cf.ByAutomationId("WelcomeLabel"));

            label!.Name.Should().Contain("manager").And.Contain("менеджер");

            ScreenshotWindow(welcome, "Test5_SuccessManager");
            welcome.Close();
        }

        [Fact]
        public void Authorization_OnlyPassword_ShowsLoginError()
        {
            _window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordTextBox"))!
                   .AsTextBox().Enter("123");
            _window.FindFirstDescendant(cf => cf.ByName("Авторизоваться"))!
                   .AsButton().Click();

            Thread.Sleep(1500);
            ScreenshotScreen("Test6_OnlyPassword");

            Assert.True(File.Exists("Test6_OnlyPassword.png"));
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

            Thread.Sleep(1500);

            var welcome = _app.GetAllTopLevelWindows(_automation)
                              .FirstOrDefault(w => w.Name == "WelcomeWindow");

            welcome.Should().NotBeNull();

            var label = welcome!
                .FindFirstDescendant(cf => cf.ByAutomationId("WelcomeLabel"));

            label!.Name.Should().Contain("admin").And.Contain("админ");

            ScreenshotWindow(welcome, "Test7_SuccessAdmin");
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