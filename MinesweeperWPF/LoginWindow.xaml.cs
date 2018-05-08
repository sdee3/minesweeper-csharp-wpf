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
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
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
                    MainWindow mainWindow = new MainWindow();

                    if ((bool)(easyRb.IsChecked))
                    {
                        mainWindow.SetGameInfo(playerName, 10, 30);
                        mainWindow.Show();
                    }
                    else if ((bool)(mediumRb.IsChecked))
                    {
                        mainWindow.SetGameInfo(playerName, 12, 45);
                        mainWindow.Show();
                    }
                    else if ((bool)(hardRb.IsChecked))
                    {
                        mainWindow.SetGameInfo(playerName, 14, 60);
                        mainWindow.Show();
                    }
                }
            }
            else
                playButton.Content = "Please select a difficulty before playing!";
        }
    }
}
