using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Z0key.Models;
using Z0key.Services;

namespace Z0key
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private VerifyUser _user;
        private UserServices _userServices;
        public LogInWindow()
        {
            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";
            if (!Directory.Exists(fileDirectory))
            {
                SetUpWindow1 setup = new SetUpWindow1();
                setup.Show();
                this.Close();
            }
            InitializeComponent();
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            _user = new VerifyUser(txtUserName.Text, pwbPassword.Password);
            _userServices = new UserServices(_user);

            Task<string> tempTask = _userServices.VerifyUser();
            string result = await tempTask;

            if (result != null)
            {
                Errors.Text = result;
            }

            HomePage window = new HomePage();
            window.Show();
            this.Close();
        }

        private void TxtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text.Length > 0)
                tbUsername.Visibility = Visibility.Collapsed;
            else
                tbUsername.Visibility = Visibility.Visible;
        }

        private void PwbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwbPassword.Password.Length > 0)
                tbPassword.Visibility = Visibility.Collapsed;
            else
                tbPassword.Visibility = Visibility.Visible;
        }
    }

}
