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

namespace Z0key
{
    /// <summary>
    /// Interaction logic for SetUpWindow1.xaml
    /// </summary>
    public partial class SetUpWindow1 : Window
    {
        private readonly Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();
        public SetUpWindow1()
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

            SetUpWindow2 window = new SetUpWindow2();
            window.Show();
            this.Close();
        }
    }

}
