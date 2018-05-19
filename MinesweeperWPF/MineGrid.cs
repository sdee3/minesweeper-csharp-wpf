using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MinesweeperWPF
{
    class MineGrid
    {
        public MinesweeperButton[,] ButtonArray { get; set; }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public int MineCount { get; private set; }
        public bool IsMine { get; private set; }
        private Random random;

        public MineGrid(int columns, int rows, int mineCount)
        {
            this.random = new Random();

            this.ColumnCount = columns;
            this.RowCount = rows;
            this.MineCount = mineCount;

            this.ButtonArray = new MinesweeperButton[this.ColumnCount, this.RowCount];
            for (int i = 0; i < this.ColumnCount; i++)
            {
                for (int j = 0; j < this.RowCount; j++)
                {
                    this.ButtonArray[i, j] = new MinesweeperButton();
                    this.ButtonArray[i, j].Name = "Button" + i.ToString() + "_" + j.ToString();
                    this.ButtonArray[i, j].Content = ""; // clears flag or bomb image (if any)
                    this.ButtonArray[i, j].Width = this.ButtonArray[i, j].Height = 30;
                    this.ButtonArray[i, j].Click += BoardButton_Click;
                    this.ButtonArray[i, j].IsEnabled = true; // button gets clickable
                    this.ButtonArray[i, j].GotFocus += BoardButton_Focus;
                }
            }

            // Generate mines
            List<String> options = new List<string>();

            for (int i = 0; i < this.ColumnCount; i++)
                for (int j = 0; j < this.RowCount; j++)
                    options.Add(i.ToString() + "," + j.ToString());

            for(int n = 0; n < this.MineCount; n++)
            {
                int index = random.Next(options.Count);
                int column = int.Parse(((options[index]).Split(',')[0]));
                int row = int.Parse(((options[index]).Split(',')[1]));
                options.RemoveAt(index);

                ButtonArray[column, row].SetMineOnButton();
            }
        }

        private void BoardButton_Focus(object sender, RoutedEventArgs e)
        {
            
        }

        private void BoardButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
