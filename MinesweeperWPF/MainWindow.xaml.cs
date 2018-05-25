using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MinesweeperWPF
{
    public partial class MainWindow : Window
    {
        private static int columnCount;
        private static int rowCount;
        private static int mineCount;
        private static int secondsElapsed;
        private static string playerName;
        private static Random random;
        private static MineGrid game;
        private static Leaderboard leaderboardObject;

        private static DispatcherTimer gameTimer;

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

            gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 1);

            random = new Random();

            for (int i = 0; i < rowCount; i++)
                minesweeperGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < columnCount; i++)
                minesweeperGrid.ColumnDefinitions.Add(new ColumnDefinition());

            SetupAndDraw();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            timerTextBlock.Text = (++secondsElapsed).ToString();
        }

        private void SetupAndDraw()
        {
            secondsElapsed = 0;
            timerTextBlock.Text = "0";
            game = new MineGrid(columnCount, rowCount, mineCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Grid.SetColumn(game.ButtonArray[i,j], j);
                    Grid.SetRow(game.ButtonArray[i, j], i);
                    game.ButtonArray[i, j].Click += BoardButton_Click;
                    game.ButtonArray[i, j].GotFocus += BoardButton_Focus;
                    minesweeperGrid.Children.Add(game.ButtonArray[i, j]);
                }
            }

            remainingMinesTextBlock.Text = game.FlagCount.ToString();
            gameTimer.Start();
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
                // If the target button is not flagged and the EnableFlagButton is turned ON
                if(game.FlagCount > 0)
                {
                    remainingMinesTextBlock.Text = game.DecrementFlagCounter().ToString();
                    toggledButton.ToggleFlagOnButton();
                    toggledButton.Content = stackPanel;
                    buttonReset.Content = FindResource("neutral_emoji");

                    if (game.FlagCount == 0)
                    {
                        remainingMinesTextBlock.Foreground = Brushes.IndianRed;
                        remainingMinesImage.Source = ImageWorker.GenerateImage(@"..\..\Assets\badflag.png");
                    }   
                }      
                else
                {
                    MessageBox.Show("You have no more flags left.");
                    buttonReset.Content = FindResource("neutral_emoji");
                }
            }
            else if(!toggledButton.IsFlagged)
            {
                // If the target button is not flagged and the EnableFlagButton is turned OFF
                toggledButton.ToggleRevealed();

                if (toggledButton.SurroundingMineCount == 0) game.RevealAllBlanks(toggledButton);

                if (toggledButton.IsMine)
                {
                    buttonReset.Content = FindResource("mineclicked_emoji");
                    gameTimer.Stop();
                    game.GameOver();
                    StartAnimation();
                }
                else
                {
                    buttonReset.Content = FindResource("neutral_emoji");

                    if (GameWon())
                    {
                        buttonReset.Content = FindResource("gameover_emoji");
                        gameTimer.Stop();
                        game.GameOver();

                        leaderboardObject = new Leaderboard(playerName, mineCount.ToString(), timerTextBlock.Text);
                        leaderboardObject.WriteDataToFile();

                        StartAnimation();
                    }
                } 
            }
            else if(toggledButton.IsFlagged && (enableFlagButton.IsChecked ?? false))
            {
                // If the target button IS FLAGGED and the EnableFlagButton is turned ON
                toggledButton.ToggleFlagOnButton();
                remainingMinesTextBlock.Text = game.IncrementFlagCounter().ToString();
                remainingMinesTextBlock.Foreground = Brushes.DarkGreen;
                toggledButton.Content = "";
                buttonReset.Content = FindResource("neutral_emoji");

                remainingMinesImage.Source = ImageWorker.GenerateImage(@"..\..\Assets\goodflag.png");
            }
        }

        private void StartAnimation()
        {
            Storyboard storyboard = this.FindResource("GameOverStoryboard") as Storyboard;
            Storyboard.SetTarget(storyboard, this.buttonReset);
            storyboard.Begin();
        }

        private bool GameWon()
        {
            for (int i = 0; i < game.RowCount; i++)
                for (int j = 0; j < game.ColumnCount; j++)
                    if (!game.ButtonArray[i, j].IsRevealed && !game.ButtonArray[i, j].IsMine)
                        return false;
            return true;
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
            MessageBox.Show(File.ReadAllText("leaderboard.dat"));
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            buttonReset.Content = FindResource("neutral_emoji");
            SetupAndDraw();
        }
    }
}
