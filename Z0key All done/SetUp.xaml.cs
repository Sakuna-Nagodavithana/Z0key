using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for SetUp.xaml
    /// </summary>
    public partial class SetUp : Window
    {
        private readonly Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();
        private User _user;
        private UserServices _userServices;
        public SetUp()
        {
            InitializeComponent();
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {

            string safefiles = "";
            _dlg.Multiselect = true;

            _dlg.Title = "Select the Files";
            var result = _dlg.ShowDialog();



            if (result == true)
            {

                foreach (string safefile in _dlg.SafeFileNames)
                {
                    safefiles += $"{safefile} \n";
                }

                FileList.Content = safefiles;

            }


        }

        private void FinishButton_OnClick(object sender, RoutedEventArgs e)
        {
            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            for (int i = 0; i < _dlg.FileNames.Length; i++)
            {
                File.Move(_dlg.FileNames[i], $"{fileDirectory}\\{_dlg.SafeFileNames[i]}");
            }

            

            TabControl.SelectedIndex = 2;
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
            else
            {
                Errors.Text = "Please Wait";
                _userServices.CreatUser();

                TabControl.SelectedIndex = 0;
            }

            

        }

        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            LogIn window = new LogIn();
            window.Show();

            _userServices.CloseApp();
            this.Close();

        }
    
    }
}
