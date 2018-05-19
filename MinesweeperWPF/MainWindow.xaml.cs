using Minesweeper.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinesweeperWPF
{
    public partial class MainWindow : Window
    {
        private static int gridSize;
        private static int mineCount;
        private static string playerName;
        private static Random random;

        private bool gameStarted;
        private Color[] mineText;

        public MainWindow()
        {
            InitializeComponent();

            gameStarted = false;
            mineText = new Color[] {Colors.White /* first doesn't matter */,
                                    Colors.Blue, Colors.DarkGreen, Colors.Red, Colors.DarkBlue,
                                    Colors.DarkViolet, Colors.DarkCyan, Colors.Brown, Colors.Black };
        }

        public void SetGameInfo(string name, int gridS, int mineC)
        {
            playerName = name;
            gridSize = gridS;
            mineCount = mineC;

            random = new Random();

            SetupAndDraw();
        }

        private void SetupAndDraw()
        {
            MineGrid game = new MineGrid(gridSize, gridSize, mineCount);

            for (int i = 0; i < gridSize; i++)
                minesweeperGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gridSize; i++)
                minesweeperGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Grid.SetColumn(game.ButtonArray[i,j], j);
                    Grid.SetRow(game.ButtonArray[i, j], i);
                    minesweeperGrid.Children.Add(game.ButtonArray[i, j]);
                }
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BoardButton_Focus(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsFocused)
                buttonReset.Content = FindResource("img_mineopening_emoji");
            else
                buttonReset.Content = FindResource("img_neutral_emoji");
        }

        private void enableFlagButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void enableFlagButton_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void buttonLeaderboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            SetupAndDraw();
        }
    }
}
