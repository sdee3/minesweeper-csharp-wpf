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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int gridSize;
        private static int mineCount;
        private static string playerName;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetGameInfo(string name, int gridS, int mineC)
        {
            playerName = name;
            gridSize = gridS;
            mineCount = mineC;

            DrawGrid();
        }

        private void DrawGrid()
        {
            int count = 1;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button MyControl1 = new Button();
                    MyControl1.Name = "Button" + count.ToString();
                    MyControl1.FontSize = 11;
                    MyControl1.Width = 60;
                    MyControl1.Height = 80;

                    Grid.SetColumn(MyControl1, j);
                    Grid.SetRow(MyControl1, i);
                    minesweeperGrid.Children.Add(MyControl1);

                    count++;
                }
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
        }
    }
}
