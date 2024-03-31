using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Z0key.Models;
using Z0key.Services;

namespace Z0key
{

    public partial class LogIn : Window
    {

        private VerifyUser _user;
        private UserServices _userServices;

        public ObservableCollection<FileItem> FileItems { get; set; }
        private readonly Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();
        public bool LogedIn = false;

        public LogIn()
        {
            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";
            if (!Directory.Exists(fileDirectory))
            {
                SetUp setup = new SetUp();
                setup.Show();
                this.Close();
            }

            InitializeComponent();
            FileItems = new ObservableCollection<FileItem>();
            DataContext = this;
        }

        private void FileListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {


            if (fileListBox.SelectedItem is FileItem selectedFile)
            {

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                string filePath = Path.Combine(folderPath, selectedFile.FileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(filePath)
                        {
                            UseShellExecute = true
                        };
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("File does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }


        private void ReomveFiles_OnClick(object sender, RoutedEventArgs e)
        {
            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";
            _dlg.Multiselect = true;
            _dlg.InitialDirectory = fileDirectory;

            _dlg.Title = "Remove the Files";
            var result = _dlg.ShowDialog();


        
            if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\RemovedFiles"))
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\RemovedFiles");
            }

            if (result == true)
            {

                for (int i = 0; i < _dlg.FileNames.Length; i++)
                {
                    File.Move($"{fileDirectory}\\{_dlg.SafeFileNames[i]}", $"{Directory.GetCurrentDirectory()}\\RemovedFiles\\{_dlg.SafeFileNames[i]}");

                }
                ReloadFileList();

            }


        }

        private void AddFiles_OnClick(object sender, RoutedEventArgs e)
        {
            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";
            _dlg.Multiselect = true;

            _dlg.Title = "Select the Files";
            var result = _dlg.ShowDialog();



            if (result == true)
            {

                for (int i = 0; i < _dlg.FileNames.Length; i++)
                {
                    File.Move(_dlg.FileNames[i], $"{fileDirectory}\\{_dlg.SafeFileNames[i]}");
                }
                ReloadFileList();

            }


        }

        private void ReloadFileList()
        {
            FileItems.Clear();

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    FileItems.Add(new FileItem
                    {
                        FileName = Path.GetFileName(file),
                        ImagePath = "Image/External/folder.png"
                    });
                }
            }
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {

            _user = new VerifyUser(txtUserName.Text, pwbPassword.Password);
            _userServices = new UserServices(_user);

            Task<string?> tempTask = _userServices.VerifyUser();
            string? result = await tempTask;

            if (result != null)
            {
                Errors.Text = result;
            }
            else
            {
                HomePage.IsEnabled = true;
                HelpPage.IsEnabled = true;
                UserPage.IsEnabled = true;
                UserPage.Opacity = 100;
                TabControl.SelectedIndex = 0;
                LogedIn = true;
                LastLoginTime.Text = $"Last Modified Time{_userServices.LastModifiedTime}";
                ReloadFileList();
            }
            

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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (LogedIn)
            {
                _userServices.CloseApp();
            }
            
        }


        private void PwdNewPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            if (pwdNewPassword.Password.Length > 0)
                tbNewPassword.Visibility = Visibility.Collapsed;
            else
                tbNewPassword.Visibility = Visibility.Visible;
        }

        private void PwbOldPassword_OnPasswordChangedPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwbOldPassword.Password.Length > 0)
                tbOldPassword.Visibility = Visibility.Collapsed;
            else
                tbOldPassword.Visibility = Visibility.Visible;
        }

        private void DeleteUser_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteUser user = new DeleteUser(_user.UserName, _user.Password);
            

            string fileDirectory = $"{Directory.GetCurrentDirectory()}\\Files";
            
            Directory.Move(fileDirectory, $"{Directory.GetCurrentDirectory()}\\Files_from_delete_user");

            _userServices.DeleteUser(user);
            MessageBox.Show("User Deleted");
            this.Close();
        }

        private void SubmitPwdChange_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeUserPassword user =
                new ChangeUserPassword(_user.UserName, pwbOldPassword.Password, pwdNewPassword.Password);

            if (user.OldPassword == _user.Password)
            {
                _userServices.ChangeUserPassword(user);
                Errors_user.Foreground = Brushes.AliceBlue;
                Errors_user.Text = "You're Password changed";
                
            }
            else
            {
                Errors_user.Text = "You're Old password does is not correct";
            }
            
        }
    }

}
