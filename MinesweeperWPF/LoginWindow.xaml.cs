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

namespace MinesweeperWPF
{
    public partial class LoginWindow : Window
    {
        private string playerName;

        public LoginWindow()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)(easyRb.IsChecked) || (bool)(mediumRb.IsChecked) || (bool)(hardRb.IsChecked))
            {
                if (textBoxUsername.Text.Length == 0)
                    playButton.Content = "Please enter a name!";
                else
                {
                    playerName = textBoxUsername.Text;

                    if ((bool)(easyRb.IsChecked))
                    {
                        ConfigAndOpenMainWindow(10, 10, 30);
                    }
                    else if ((bool)(mediumRb.IsChecked))
                    {
                        ConfigAndOpenMainWindow(12, 12, 45);
                    }
                    else if ((bool)(hardRb.IsChecked))
                    {
                        ConfigAndOpenMainWindow(14, 14, 60);
                    }
                }
            }
            else
                playButton.Content = "Please select a difficulty before playing!";
        }

        void ConfigAndOpenMainWindow(int columnCount, int rowCount, int mineCount)
        {
            MainWindow mainWindow = new MainWindow(playerName);

            mainWindow.SetGameInfo(columnCount, rowCount, mineCount);
            mainWindow.Show();
            mainWindow.Closing += (s, args) => this.Close();
            this.Hide();
        }
    }
}
