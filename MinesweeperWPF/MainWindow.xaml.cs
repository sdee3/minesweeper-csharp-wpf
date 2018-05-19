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
        private static List<int> mineList;
        private static List<Button> allButtons;
        private static Random random;

        public MinesGrid Mines { get; private set; }
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

            mineList = new List<int>(mineC);
            allButtons = new List<Button>();
            random = new Random();

            GameSetup();
        }

        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            GameSetup();
        }

        private void GameSetup()
        {
            Mines = new MinesGrid(gridSize, gridSize, 1, gridSize);
            allButtons = new List<Button>();

            for (int i = 0; i < gridSize * gridSize; i++)
                allButtons.Add(new Button());

            foreach (Button btn in allButtons)
            {
                btn.Content = ""; // clears flag or bomb image (if any)
                btn.Width = 30;
                btn.Height = 30;
                btn.Click += Button_Click;
                btn.IsEnabled = true; // button gets clickable
                btn.GotFocus += BoardButton_Focus;
                btn.LostFocus += BoardButton_Focus;
            }

            int tmpBrojac = 0;

            for(int i = 0; i < gridSize; i++)
            {
                for(int j=0; j < gridSize; j++)
                {
                    allButtons[tmpBrojac].Name = "Button" + i.ToString() + j.ToString();
                    Grid.SetColumn(allButtons[tmpBrojac], j);
                    Grid.SetRow(allButtons[tmpBrojac], i);
                    minesweeperGrid.Children.Add(allButtons[tmpBrojac++]);
                }
            }
            // Attaches Mines Indicator Event
            Mines.CounterChanged += OnCounterChanged;

            // Attaches Button Click, invoked by a plate
            Mines.ClickPlate += OnClickPlate;

            // Attaches Time Threshold Elapsed Event
            Mines.TimerThresholdReached += OnTimeChanged;
            timerTextBlock.Text = "0";

            Mines.Run();
            gameStarted = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender; // gets clicked button reference
            int row = ParseButtonRow(btn);
            int col = ParseButtonColumn(btn);
            if (!Mines.IsInGrid(row, col)) throw new MinesweeperException("Invalid Button to MinesGrid reference on reveal"); // the button points to an invalid plate

            if (Mines.IsFlagged(row, col)) return; // flagged plate cannot be revealed

            btn.IsEnabled = false; // disables the button
            if (Mines.IsBomb(row, col)) //a bomb was revealed !!! 
            {
                // attaches bomb image to the button
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image bombImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"..\..\Assets\mine.gif", UriKind.Relative);
                bi.EndInit();
                bombImage.Source = bi;
                sp.Children.Add(bombImage);
                btn.Content = sp;
                Mines.Stop();

                // finishes the game and opens all plates
                if (gameStarted)
                {
                    gameStarted = false;
                    foreach (Button butn in minesweeperGrid.Children)
                    {
                        if (butn.IsEnabled) this.Button_Click(butn, e); // calls all other unrevealed buttons
                    }
                }
            }
            else // an empty space was revealed
            {
                int count = Mines.RevealPlate(row, col); // opens the plate and checks for surrounding bombs
                if (count > 0) // put a corresponding label on the current button
                {
                    btn.Foreground = new SolidColorBrush(mineText[count]);
                    btn.FontWeight = FontWeights.Bold;
                    btn.Content = count.ToString();
                }
            }
        }

        private void Flag_Click(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender; // gets clicked button reference
            int row = ParseButtonRow(btn);
            int col = ParseButtonColumn(btn);
            if (!Mines.IsInGrid(row, col)) throw new MinesweeperException("Invalid Button to MinesGrid reference on flag"); // the button points to an invalid plate

            if (Mines.IsFlagged(row, col)) // the button has flag image child
            {
                btn.Content = ""; // clears flag image
            }
            else
            {
                // attaches flag image to the button
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image flagImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"..\..\Assets\flag.gif", UriKind.Relative);
                bi.EndInit();
                flagImage.Source = bi;
                sp.Children.Add(flagImage);
                btn.Content = sp;
            }

            Mines.FlagMine(row, col);
        }

        private int ParseButtonRow(Button btn)
        {
            // Button Name format must be "ButtonXY" or "ButtonXXYY", where X and Y are numberical indices of the mine cell
            if (btn.Name.IndexOf("Button") != 0) throw new MinesweeperException("Wrong button name in UI module"); // the button is wrong
            return int.Parse(btn.Name.Substring(6, (btn.Name.Length - 6) / 2));
        }

        private int ParseButtonColumn(Button btn)
        {
            // Button Name format must be "ButtonXY" or "ButtonXXYY", where X and Y are numberical indices of the mine cell
            if (btn.Name.IndexOf("Button") != 0) throw new MinesweeperException("Wrong button name in UI module"); // the button is wrong
            return int.Parse(btn.Name.Substring(6 + (btn.Name.Length - 6) / 2, (btn.Name.Length - 6) / 2));
        }

        private void OnCounterChanged(object sender, EventArgs e)
        {
            // Updates MineIndicator field in the UI
            remainingMinesTextBlock.Text = (mineCount - Mines.FlaggedMines).ToString();
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            // Updates MineIndicator field in the UI
            remainingMinesTextBlock.Text = Mines.TimeElapsed.ToString();
        }

        private void OnClickPlate(object sender, PlateEventArgs e)
        {
            // Opens requested plate through simulating Button Click
            Button senderButton = new Button();
            string btnName = "Button";
            btnName += String.Format("{0:D1}{1:D1}", e.PlateColumn, e.PlateRow); // double digit coordinates

            foreach(Button b in allButtons)
                if(b.Name.Equals(btnName))
                {
                    senderButton = b;
                    break;
                }

            if (senderButton == null) throw new MinesweeperException("Invalid Button to MinesGrid reference on multiple reveal"); // the plate refers to an invalid button

            // calls respective "Button Click" event handler 
            this.Button_Click(senderButton, new RoutedEventArgs());
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
            GameSetup();
        }
    }
}
