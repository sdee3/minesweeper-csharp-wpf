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
        private static int columnCount;
        private static int rowCount;
        private static int mineCount;
        private static string playerName;
        private static Random random;
        private static MineGrid game;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string pName) : this()
        {
            playerName = pName;
        }

        public void SetGameInfo(int cols, int rows, int mineC)
        {
            columnCount = cols;
            rowCount = rows;
            mineCount = mineC;

            random = new Random();

            for (int i = 0; i < columnCount; i++)
                minesweeperGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rowCount; i++)
                minesweeperGrid.RowDefinitions.Add(new RowDefinition());

            SetupAndDraw();
        }

        private void SetupAndDraw()
        {
            game = new MineGrid(columnCount, rowCount, mineCount);

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    Grid.SetColumn(game.ButtonArray[i,j], j);
                    Grid.SetRow(game.ButtonArray[i, j], i);
                    game.ButtonArray[i, j].Click += BoardButton_Click;
                    game.ButtonArray[i, j].GotFocus += BoardButton_Focus;
                    minesweeperGrid.Children.Add(game.ButtonArray[i, j]);
                }
            }
            
        }

        private void BoardButton_Click(object sender, RoutedEventArgs e)
        {
            MinesweeperButton toggledButton = sender as MinesweeperButton;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            Image flagImage = new Image();
            flagImage.Source = ImageWorker.GenerateImage(@"..\..\Assets\flag.gif");

            stackPanel.Children.Add(flagImage);

            if (!toggledButton.IsFlagged && (enableFlagButton.IsChecked ?? false))
            {
                toggledButton.ToggleFlagOnButton();
                toggledButton.Content = stackPanel;
                buttonReset.Content = FindResource("neutral_emoji");
            }
            else if(!toggledButton.IsFlagged)
            {
                toggledButton.ToggleRevealed();
                if (toggledButton.SurroundingMineCount == 0) game.RevealAllBlanks(toggledButton);
                if (toggledButton.IsMine)
                {
                    buttonReset.Content = FindResource("mineclicked_emoji");
                    game.GameOver();
                }
                else
                    buttonReset.Content = FindResource("neutral_emoji");
            }
            else if(toggledButton.IsFlagged && (enableFlagButton.IsChecked ?? false))
            {
                toggledButton.ToggleFlagOnButton();
                toggledButton.Content = "";
                buttonReset.Content = FindResource("neutral_emoji");
            }
        }

        private void BoardButton_Focus(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsFocused)
                buttonReset.Content = FindResource("mineopening_emoji");
        }

        private void enableFlagButton_Checked(object sender, RoutedEventArgs e)
        {
            enableFlagButton.Content = FindResource("goodflag");
        }

        private void enableFlagButton_Unchecked(object sender, RoutedEventArgs e)
        {
            enableFlagButton.Content = FindResource("flag");
        }

        private void buttonLeaderboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            buttonReset.Content = FindResource("neutral_emoji");
            SetupAndDraw();
        }
    }
}
