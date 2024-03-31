using System;
using System.Collections.Generic;
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
    /// Interaction logic for SetUpWindow1.xaml
    /// </summary>
    public partial class SetUpWindow2 : Window
    {
        private User _user;
        private UserServices _userServices;
        public SetUpWindow2()
        {
            InitializeComponent();
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

        private void PwbConformPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwbConformPassword.Password.Length > 0)
                tbConformPassword.Visibility = Visibility.Collapsed;
            else
                tbConformPassword.Visibility = Visibility.Visible;
        }

        private void TxtEmail_OnTextChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
                tbEmail.Visibility = Visibility.Collapsed;
            else
                tbEmail.Visibility = Visibility.Visible;
        }


        private void SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            _user = new User(txtUserName.Text,
                pwbPassword.Password, pwbConformPassword.Password, txtEmail.Text);


            _userServices = new UserServices(_user);




            if (_userServices.CheckUser() != null)
            {
                Errors.Text = _userServices.CheckUser();
            }

            Errors.Text = "Please Wait";
            _userServices.CreatUser();


            
            SetUpWindow3 window = new SetUpWindow3();
            window.Show();
            this.Close();
            
        }
    }
}
