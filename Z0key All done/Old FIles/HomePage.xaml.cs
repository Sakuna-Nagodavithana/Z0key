using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Z0key.Models;
using Path = System.IO.Path;

namespace Z0key
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    ///



    public partial class HomePage : Window
    {
        public ObservableCollection<FileItem> FileItems { get; set; }
        private readonly Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();

        public HomePage()
        {
            InitializeComponent();
            FileItems = new ObservableCollection<FileItem>();

            ReloadFileList();
        }

        private void Help_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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



            if (result == true)
            {

                for (int i = 0; i < _dlg.FileNames.Length; i++)
                {
                    File.Delete(_dlg.FileNames[i]);

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
                        ImagePath = "/folder.png"
                    });
                }
            }
        }
    }
}
