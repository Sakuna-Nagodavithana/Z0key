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

namespace Z0key
{
    /// <summary>
    /// Interaction logic for SetUpWindow3.xaml
    /// </summary>
    public partial class SetUpWindow3 : Window
    {
        public SetUpWindow3()
        {
            InitializeComponent();
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            LogIn window = new LogIn();
            window.Show();
            this.Close();
        }
    }
}
