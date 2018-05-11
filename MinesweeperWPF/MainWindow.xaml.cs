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
        private static Random random;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetGameInfo(string name, int gridS, int mineC)
        {
            playerName = name;
            gridSize = gridS;
            mineCount = mineC;

            mineList = new List<int>(mineC);
            random = new Random();

            DrawGrid();
        }

        private void DrawGrid()
        {
            int count = 1;
            int tmpCounter = 0;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button button = new Button();
                    button.Name = "Button" + count.ToString();
                    button.FontSize = 11;
                    button.Width = 35;
                    button.Height = 40;
                    button.GotFocus += BoardButton_Focus;
                    button.LostFocus += BoardButton_Focus;
                    button.Click += BoardButton_Click;

                    if((random.Next(100)) > 50 && tmpCounter < 60)
                    {
                        button.Content = "mine";
                        mineList.Add(count);
                        tmpCounter++;
                    }

                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    minesweeperGrid.Children.Add(button);

                    count++;
                }
            }
        }

        private void BoardButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BoardButton_Focus(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsFocused)
                buttonReset.Content = FindResource("img_mineopening_emoji");
            if (! ((Button)sender).IsFocused)
                buttonReset.Content = FindResource("img_neutral_emoji");
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Console.WriteLine(b.Name);
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
            buttonReset.Content = FindResource("img_mineopening_emoji");
        }
    }
}
