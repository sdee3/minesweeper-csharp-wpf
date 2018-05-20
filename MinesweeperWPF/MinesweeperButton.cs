using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MinesweeperWPF
{
    class MinesweeperButton : Button
    {
        public bool IsMine { get; private set; }
        public bool IsFlagged { get; private set; }
        public bool IsRevealed { get; private set; }
        public int SurroundingMineCount { get; set; }
        public int RowValue { get; set; }
        public int ColumnValue { get; set; }

        public MinesweeperButton()
        {
            this.IsMine = false;
            this.IsFlagged = false;
            this.IsRevealed = false;
            this.SurroundingMineCount = 0;
        }

        public void SetMineOnButton()
        {
            this.IsMine = true;
        }

        public void ToggleFlagOnButton()
        {
            this.IsFlagged = this.IsFlagged ? false : true;
        }

        public void ToggleRevealed()
        {
            if(!this.IsFlagged)
            {
                this.IsRevealed = true;

                if (this.IsMine)
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    Image mineImage = new Image();
                    mineImage.Source = ImageWorker.GenerateImage(@"..\..\Assets\mine.gif");

                    stackPanel.Children.Add(mineImage);

                    this.Content = stackPanel;
                }
                else this.Content = (this.SurroundingMineCount > 0) ? (this.SurroundingMineCount.ToString()) : "";

                this.Background = this.IsMine ? Brushes.IndianRed : Brushes.DarkGray;
            }   
        }

        public void CountMines(int ColumnCount, int RowCount, MinesweeperButton[,] ButtonArray)
        {
            if (this.IsMine)
            {
                this.SurroundingMineCount = -1;
                return;
            }

            int totalMineCount = 0;

            for (int xoff = -1; xoff <= 1; xoff++)
            {
                int i = this.ColumnValue + xoff;
                if (i < 0 || i >= ColumnCount) continue;

                for (int yoff = -1; yoff <= 1; yoff++)
                {
                    int j = this.RowValue + yoff;
                    if (j < 0 || j >= RowCount) continue;

                    MinesweeperButton neighbor = ButtonArray[i, j];
                    if (neighbor.IsMine) totalMineCount++;
                }
            }

            this.SurroundingMineCount = totalMineCount;
        }
    }
}
